using Authorization.Application.Options;
using Authorization.Application.Services.Interfaces.Authentication;
using Authorization.Application.Services.Interfaces.JWT;
using Authorization.Application.Services.Interfaces.TokenProviers;
using Authorization.Application.Utility;
using Authorization.Core.Dto.Request;
using Authorization.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.Exceptions;
using System.Security.Claims;

namespace Authorization.Application.Services.Implementations.Authentication;

public class AccessService : IAccessService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtProvider _jwtProvider;
    private readonly IOptions<SecretKeys> _keys;
    private readonly IRefreshProvider _refreshProvider;

    public AccessService(
        IAccountRepository accountRepository,
        IPasswordService passwordHasher,
        IJwtProvider jwtProvider,
        IOptions<SecretKeys> keys,
        IRefreshProvider refreshProvider)
    {
        _accountRepository = accountRepository;
        _passwordService = passwordHasher;
        _jwtProvider = jwtProvider;
        _keys = keys;
        _refreshProvider = refreshProvider;
    }

    public async Task<Tokens> LoginAsync(LoginAccountRequestDto loginAccountRequestDto)
    {
        var account = await _accountRepository.GetByEmailAsync(loginAccountRequestDto.Email, trackChanges: true);

        if (account == null)
        {
            throw new NotFoundException(nameof(account), loginAccountRequestDto.Email);
        }

        var verificationResult = _passwordService.Verify(account.Password, loginAccountRequestDto.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException($"Incorrect creditionals");
        }

        if (!account.IsEmailVerified)
        {
            throw new UnauthorizedException($"Email {account.Email} is not verified");
        }

        var claimsIdentity = new ClaimsIdentity([new Claim("id", account.Id.ToString())]);

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
