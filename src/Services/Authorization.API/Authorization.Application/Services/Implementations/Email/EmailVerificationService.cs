using Authorization.Application.Options;
using Authorization.Application.Services.Interfaces.Email;
using Authorization.Application.Services.Interfaces.JWT;
using Authorization.Core.Repositories;
using Microsoft.Extensions.Options;
using Shared.Exceptions;

namespace Authorization.Application.Services.Implementations.Email;

public class EmailVerificationService : IEmailVerificationService
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IAccountRepository _accountRepository;
    private readonly IOptions<SecretKeys> _keys;

    public EmailVerificationService(
        IJwtProvider jwtProvider,
        IAccountRepository accountRepository,
        IOptions<SecretKeys> keys)
    {
        _jwtProvider = jwtProvider;
        _accountRepository = accountRepository;
        _keys = keys;
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
}
