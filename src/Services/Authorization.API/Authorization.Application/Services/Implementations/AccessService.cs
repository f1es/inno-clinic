using Authorization.Application.Extensions;
using Authorization.Application.Options;
using Authorization.Application.Services.Interfaces;
using Authorization.Application.Utility;
using Authorization.Core.Dto.Request;
using Authorization.Core.Models;
using Authorization.Core.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.Exceptions;
using System.Security.Claims;

namespace Authorization.Application.Services.Implementations;

public class AccessService : IAccessService
{
	private readonly IAccountRepository _accountRepository;
	private readonly IPasswordService _passwordHasher;
	private readonly IJwtProvider _jwtProvider;
	private readonly IValidator<RegisterAccountRequestDto> _validator;
	private readonly IOptions<SecretKeys> _keys;
	private readonly IEmailSender _emailSender;
	private readonly IRefreshProvider _refreshProvider;

	public AccessService(
		IAccountRepository accountRepository,
		IPasswordService passwordHasher,
		IJwtProvider jwtProvider,
		IValidator<RegisterAccountRequestDto> validator,
		IOptions<SecretKeys> keys,
		IEmailSender emailSender,
		IRefreshProvider refreshProvider)
	{
		_accountRepository = accountRepository;
		_passwordHasher = passwordHasher;
		_jwtProvider = jwtProvider;
		_validator = validator;
		_keys = keys;
		_emailSender = emailSender;
		_refreshProvider = refreshProvider;
	}

	public async Task<Tokens> LoginAsync(LoginAccountRequestDto loginAccountRequestDto)
	{
		var account = await _accountRepository.GetByEmailAsync(loginAccountRequestDto.Email, trackChanges: true);

		if (account == null)
		{
			throw new NotFoundException(nameof(account), loginAccountRequestDto.Email);
		}

		var verificationResult = _passwordHasher.Verify(account.Password, loginAccountRequestDto.Password);

		if (verificationResult == PasswordVerificationResult.Failed)
		{
			throw new UnauthorizedException($"Incorrect creditionals");
		}

		if (!account.IsEmailVerified)
		{
			throw new UnauthorizedException($"Email {account.Email} is not verified");
		}

		var claimsIdentity = new ClaimsIdentity( [new Claim("id", account.Id.ToString())] );

		var accessToken = _jwtProvider.GenerateToken(_keys.Value.Access, 3, claimsIdentity);
		var refreshToken = _refreshProvider.GenerateToken();

		account.RefreshToken = refreshToken;
		account.RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(31);

		await _accountRepository.SaveAsync();

		var tokens = new Tokens
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken
		};

		return tokens;
	}

	public async Task RegisterAsync(RegisterAccountRequestDto registerAccountRequestDto)
	{
		var validationResult = await _validator.ValidateAsync(registerAccountRequestDto);

		if (!validationResult.IsValid)
		{
			throw new BadRequestException($"{validationResult.GetErrors()}");
		}

		var passwordHash = _passwordHasher.Hash(registerAccountRequestDto.Password);

		var account = new Account
		{
			Email = registerAccountRequestDto.Email,
			Password = passwordHash,
			PhoneNumber = registerAccountRequestDto.PhoneNumber,
			CreatedAt = DateTime.UtcNow,
			PhotoId = registerAccountRequestDto.PhotoId,
			IsEmailVerified = false,
			CreatedBy = "Annonumys"
		};

		_accountRepository.Create(account);

		await _accountRepository.SaveAsync();

		var claimsIdentity = new ClaimsIdentity( [new Claim(ClaimTypes.Email, registerAccountRequestDto.Email)] );

		var emailToken = _jwtProvider.GenerateToken(_keys.Value.Email, 1, claimsIdentity);

		var endpoint = $"https://localhost:44366/api/accounts/email-verification/?token={emailToken}";
		var subject = "Verify your email in inno clinic";

		var message = new Message([registerAccountRequestDto.Email], subject, endpoint);

		await _emailSender.SendEmailAsync(message);
	}

	public async Task VerifyEmailAsync(string token)
	{
		var jwt = _jwtProvider.ReadToken(token);

		var email = jwt.Claims.FirstOrDefault(x => x.Type == "email").Value;

		if (email == null)
		{
			throw new BadRequestException($"Incorrect verification token");
		}

		var account = await _accountRepository.GetByEmailAsync(email, trackChanges: true);

		if (account == null)
		{
			throw new NotFoundException(nameof(account), email);
		}

		var verificationResult = await _jwtProvider.VerifyEmailTokenAsync(token, _keys.Value.Email, email);

		if (verificationResult)
		{
			account.IsEmailVerified = true;
			await _accountRepository.SaveAsync();
		}
        else
        {
			throw new BadRequestException("Email cannot be verified");
        }
    }

	public async Task<Tokens> RefreshAsync(Tokens tokens)
	{
		var accessToken = tokens.AccessToken;
		var refreshToken = tokens.RefreshToken;

		var principal = _jwtProvider.GetPrincipalFromExpiredToken(tokens.AccessToken, _keys.Value.Access);
		var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == "id");
		var userId = Guid.Parse(userIdClaim.Value);

		var account = await _accountRepository.GetByIdAsync(userId, trackChanges: true);

		if (account == null)
		{
			throw new NotFoundException(nameof(account), userId);
		}

		if (account.RefreshToken != refreshToken)
		{
			throw new BadRequestException("Invalid client request");
		}

		var claimsIdentity = new ClaimsIdentity([userIdClaim]);

		var newAccessToken = _jwtProvider.GenerateToken(_keys.Value.Access, 2, claimsIdentity);
		var newRefreshToken = _refreshProvider.GenerateToken();

		account.RefreshToken = newRefreshToken;
		account.RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(31);

		await _accountRepository.SaveAsync();

		tokens.AccessToken = newAccessToken;
		tokens.RefreshToken = newRefreshToken;

		return tokens;
	}
}
