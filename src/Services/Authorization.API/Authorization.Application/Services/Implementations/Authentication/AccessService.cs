using Authorization.Application.Options;
using Authorization.Application.Services.Interfaces.Authentication;
using Authorization.Application.Services.Interfaces.JWT;
using Authorization.Application.Services.Interfaces.TokenProviers;
using Authorization.Application.Utility;
using Authorization.Core.Dto.Request;
using Authorization.Core.Models;
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
    private readonly SecretKeys _keys;
    private readonly JwtTokenOptions _jwtTokenOptions;
    private readonly IRefreshProvider _refreshProvider;

	public AccessService(
		IAccountRepository accountRepository,
		IPasswordService passwordHasher,
		IJwtProvider jwtProvider,
		IOptions<SecretKeys> keys,
		IRefreshProvider refreshProvider,
		IOptions<JwtTokenOptions> jwtTokenOptions)
	{
		_accountRepository = accountRepository;
		_passwordService = passwordHasher;
		_jwtProvider = jwtProvider;
		_keys = keys.Value;
		_refreshProvider = refreshProvider;
		_jwtTokenOptions = jwtTokenOptions.Value;
	}

	public async Task<Tokens> LoginAsync(LoginAccountRequestDto loginAccountRequestDto)
    {
        var account = await _accountRepository.GetByEmailAsync(loginAccountRequestDto.Email, trackChanges: true);

        AccountNullCheck(account, loginAccountRequestDto.Email);

        var verificationResult = _passwordService.Verify(account.Password, loginAccountRequestDto.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException($"Incorrect creditionals");
        }

        if (!account.IsEmailVerified)
        {
            throw new UnauthorizedException($"Email {account.Email} is not verified");
        }

        var claimsIdentity = PutClaims(account.Id, account.Role);

        var accessToken = _jwtProvider.GenerateToken(_keys.Access, _jwtTokenOptions.AccessTokenLifetimeMinutes, claimsIdentity);
        var refreshToken = _refreshProvider.GenerateToken();

        account.RefreshToken = refreshToken;
        account.RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(_jwtTokenOptions.RefreshTokenLifetimeDays);

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
		TokensNullCheck(tokens);

		var principal = _jwtProvider.GetPrincipalFromExpiredToken(tokens.AccessToken, _keys.Access);
        var accountId = GetAccountIdFromPrincipal(principal);

        var account = await _accountRepository.GetByIdAsync(accountId, trackChanges: true);

		AccountNullCheck(account, accountId);

		if (account.RefreshToken != tokens.RefreshToken)
        {
            throw new BadRequestException("Invalid client request");
        }

        if (account.RefreshTokenExpirationDate < DateTime.UtcNow)
        {
            throw new BadRequestException("Refresh token is expired");
        }

		var claimsIdentity = PutClaims(accountId, account.Role);

        tokens.AccessToken = _jwtProvider.GenerateToken(_keys.Access, _jwtTokenOptions.AccessTokenLifetimeMinutes, claimsIdentity);
        tokens.RefreshToken = _refreshProvider.GenerateToken();

        account.RefreshToken = tokens.RefreshToken;
        account.RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(_jwtTokenOptions.RefreshTokenLifetimeDays);

        await _accountRepository.SaveAsync();

        return tokens;
    }

    public async Task RevokeAsync(Tokens tokens)
    {
        TokensNullCheck(tokens);

        var principal = _jwtProvider.GetPrincipalFromExpiredToken(tokens.AccessToken, _keys.Access);
        var accountId = GetAccountIdFromPrincipal(principal);

        var account = await _accountRepository.GetByIdAsync(accountId, trackChanges: true);

        AccountNullCheck(account, accountId);

        account.RefreshToken = null;
        account.RefreshTokenExpirationDate = null;

        await _accountRepository.SaveAsync();
    }

    private Guid GetAccountIdFromPrincipal(ClaimsPrincipal principal) => Guid.Parse(principal.FindFirst(x => x.Type == "id").Value);
    private Account AccountNullCheck(Account account, object key) => account ?? throw new NotFoundException(nameof(account), key);
    private void TokensNullCheck(Tokens tokens)
    {
		if (tokens.AccessToken == null || tokens.RefreshToken == null)
		{
			throw new BadRequestException("Invalid client request, missing tokens");
		}
	}
    private ClaimsIdentity PutClaims(Guid accountId, string? accountRole)
    {
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim("id", accountId.ToString()));
        if (accountRole != null)
        {
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, accountRole));
        }

        return claimsIdentity;
	}
}
