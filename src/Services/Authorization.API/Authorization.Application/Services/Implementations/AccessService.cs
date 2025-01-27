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

	public AccessService(
		IAccountRepository accountRepository,
		IPasswordService passwordHasher,
		IJwtProvider jwtProvider,
		IValidator<RegisterAccountRequestDto> validator,
		IOptions<SecretKeys> keys,
		IEmailSender emailSender)
	{
		_accountRepository = accountRepository;
		_passwordHasher = passwordHasher;
		_jwtProvider = jwtProvider;
		_validator = validator;
		_keys = keys;
		_emailSender = emailSender;
	}

	public async Task<string> LoginAsync(LoginAccountRequestDto loginAccountRequestDto)
	{
		var account = await _accountRepository.GetByEmailAsync(loginAccountRequestDto.Email);

		if (account == null)
		{
			throw new Exception("404");
			// ex 404
		}

		if (!account.IsEmailVerified)
		{
			throw new Exception("401");
		}

		var verificationResult = _passwordHasher.Verify(account.Password, loginAccountRequestDto.Password);

		if (verificationResult == PasswordVerificationResult.Failed)
		{
			throw new Exception("401");
			// ex 401
		}

		return _jwtProvider.GenerateToken(_keys.Value.Access, 3, new ClaimsIdentity());
	}

	public async Task RegisterAsync(RegisterAccountRequestDto registerAccountRequestDto)
	{
		var validationResult = await _validator.ValidateAsync(registerAccountRequestDto);

		if (!validationResult.IsValid)
		{
			// custom ex
			throw new Exception($"{validationResult.GetErrors()}");
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

		var claimsIdentity = new ClaimsIdentity(new List<Claim>()
		{
			new Claim(ClaimTypes.Email, registerAccountRequestDto.Email)
		});
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

		var account = await _accountRepository.GetByEmailAsync(email, trackChanges: true);

		if (account == null)
		{
			throw new Exception("404");
		}

		var verificationResult = await _jwtProvider.VerifyEmailTokenAsync(token, _keys.Value.Email, email);

		if (verificationResult)
		{
			account.IsEmailVerified = true;
			await _accountRepository.SaveAsync();
		}
        else
        {
			// ex 400
			throw new Exception("400");
        }
    }
}
