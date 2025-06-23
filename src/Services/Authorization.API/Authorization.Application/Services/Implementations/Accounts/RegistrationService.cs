using Authorization.Application.Extensions;
using Authorization.Application.Options;
using Authorization.Application.Services.Interfaces.Accounts;
using Authorization.Application.Services.Interfaces.Authentication;
using Authorization.Application.Services.Interfaces.Email;
using Authorization.Application.Services.Interfaces.JWT;
using Authorization.Application.Utility;
using Authorization.Core.Dto.Request;
using Authorization.Core.Models;
using Authorization.Core.Repositories;
using FluentValidation;
using Microsoft.Extensions.Options;
using Shared.Exceptions;
using Shared.Options;
using System.Security.Claims;

namespace Authorization.Application.Services.Implementations.Accounts;

public class RegistrationService : IRegistrationService
{
    private readonly IValidator<RegisterAccountRequestDto> _validator;
    private readonly IPasswordService _passwordService;
    private readonly IAccountRepository _accountRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IEmailSender _emailSender;
    private readonly JwtOptions _jwtOptions;
    private readonly AccountEndpointsOptions _endpointOptions;

	public RegistrationService(
		IValidator<RegisterAccountRequestDto> validator,
		IPasswordService passwordService,
		IAccountRepository accountRepository,
		IJwtProvider jwtProvider,
		IEmailSender emailSender,
		IOptions<AccountEndpointsOptions> endpoints,
		IOptions<JwtOptions> jwtOptions)
	{
		_validator = validator;
		_passwordService = passwordService;
		_accountRepository = accountRepository;
		_jwtProvider = jwtProvider;
		_emailSender = emailSender;
		_endpointOptions = endpoints.Value;
		_jwtOptions = jwtOptions.Value;
	}

	public async Task RegisterAsync(RegisterAccountRequestDto registerAccountRequestDto)
    {
        var validationResult = await _validator.ValidateAsync(registerAccountRequestDto);
        if (!validationResult.IsValid)
        {
            throw new BadRequestException($"{validationResult.GetErrors()}");
        }

        var emailUniqueness = await _accountRepository.GetByEmailAsync(registerAccountRequestDto.Email);
        if (emailUniqueness != null)
        {
            throw new BadRequestException($"User with email {registerAccountRequestDto.Email} already exist");
        }

        var passwordHash = _passwordService.Hash(registerAccountRequestDto.Password);
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

        var claimsIdentity = new ClaimsIdentity([new Claim(ClaimTypes.Email, registerAccountRequestDto.Email)]);
        var emailToken = _jwtProvider.GenerateToken(_jwtOptions.EmailKey, 1, claimsIdentity);
        var message = BuildMessage(emailToken, [registerAccountRequestDto.Email]);

        await _emailSender.SendEmailAsync(message);
    }

    private Message BuildMessage(string emailToken, string[] emails)
    {
		var emailVerificationUri = new UriBuilder(_endpointOptions.EmailVerification);
		emailVerificationUri.Query = $"?token={emailToken}";
		var subject = "Verify your email in inno clinic";

		return new Message(emails, subject, emailVerificationUri.ToString());
	}
}
