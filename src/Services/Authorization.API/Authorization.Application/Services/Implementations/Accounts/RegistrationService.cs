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
using System.Security.Claims;

namespace Authorization.Application.Services.Implementations.Accounts;

public class RegistrationService : IRegistrationService
{
    private readonly IValidator<RegisterAccountRequestDto> _validator;
    private readonly IPasswordService _passwordService;
    private readonly IAccountRepository _accountRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IEmailSender _emailSender;
    private readonly IOptions<SecretKeys> _keys;

    public RegistrationService(
        IValidator<RegisterAccountRequestDto> validator,
        IPasswordService passwordService,
        IAccountRepository accountRepository,
        IJwtProvider jwtProvider,
        IEmailSender emailSender,
        IOptions<SecretKeys> keys)
    {
        _validator = validator;
        _passwordService = passwordService;
        _accountRepository = accountRepository;
        _jwtProvider = jwtProvider;
        _emailSender = emailSender;
        _keys = keys;
    }

    public async Task RegisterAsync(RegisterAccountRequestDto registerAccountRequestDto)
    {
        var validationResult = await _validator.ValidateAsync(registerAccountRequestDto);

        if (!validationResult.IsValid)
        {
            throw new BadRequestException($"{validationResult.GetErrors()}");
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

        var emailToken = _jwtProvider.GenerateToken(_keys.Value.Email, 1, claimsIdentity);

        var endpoint = $"https://localhost:5006/api/accounts/email-verification/?token={emailToken}";
        var subject = "Verify your email in inno clinic";

        var message = new Message([registerAccountRequestDto.Email], subject, endpoint);

        await _emailSender.SendEmailAsync(message);
    }
}
