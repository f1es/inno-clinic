using Authorization.Application.Options;
using Authorization.Application.Services.Implementations.Authentication;
using Authorization.Application.Services.Interfaces.Authentication;
using Authorization.Application.Services.Interfaces.JWT;
using Authorization.Application.Services.Interfaces.TokenProviers;
using Authorization.Application.Utility;
using Authorization.Core.Dto.Request;
using Authorization.Core.Models;
using Authorization.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Exceptions;
using System.Security.Claims;

namespace Authorization.Tests.Services.Authentication;

public class AccessServiceTests
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly Mock<IRefreshProvider> _refreshProviderMock;
    private readonly SecretKeys _secretKeys;
    private readonly JwtTokenOptions _jwtTokenOptions;
    private readonly AccessService _accessService;

    public AccessServiceTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _jwtProviderMock = new Mock<IJwtProvider>();
        _refreshProviderMock = new Mock<IRefreshProvider>();

        _secretKeys = new SecretKeys { Access = "test-access-key" };
        _jwtTokenOptions = new JwtTokenOptions
        {
            AccessTokenLifetimeMinutes = 15,
            RefreshTokenLifetimeDays = 7
        };

        var secretKeysOptions = Options.Create(_secretKeys);
        var jwtTokenOptions = Options.Create(_jwtTokenOptions);

        _accessService = new AccessService(
            _accountRepositoryMock.Object,
            _passwordServiceMock.Object,
            _jwtProviderMock.Object,
            secretKeysOptions,
            _refreshProviderMock.Object,
            jwtTokenOptions
        );
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsTokens()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var email = "test@example.com";
        var password = "password123";
        var role = "User";
        var accessToken = "access-token";
        var refreshToken = "refresh-token";

        var account = new Account
        {
            Id = accountId,
            Email = email,
            Password = "hashed-password",
            Role = role,
            IsEmailVerified = true
        };

        var loginRequest = new LoginAccountRequestDto(
            Email: email,
            Password: password
        );

        _accountRepositoryMock.Setup(x => x.GetByEmailAsync(email, true))
            .ReturnsAsync(account);

        _passwordServiceMock.Setup(x => x.Verify(account.Password, password))
            .Returns(PasswordVerificationResult.Success);

        _jwtProviderMock.Setup(x => x.GenerateToken(
                _secretKeys.Access,
                _jwtTokenOptions.AccessTokenLifetimeMinutes,
                It.IsAny<ClaimsIdentity>()))
            .Returns(accessToken);

        _refreshProviderMock.Setup(x => x.GenerateToken())
            .Returns(refreshToken);

        // Act
        var result = await _accessService.LoginAsync(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(accessToken, result.AccessToken);
        Assert.Equal(refreshToken, result.RefreshToken);
        Assert.Equal(refreshToken, account.RefreshToken);
        Assert.NotNull(account.RefreshTokenExpirationDate);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ThrowsUnauthorizedException()
    {
        // Arrange
        var email = "test@example.com";
        var password = "wrong-password";
        var account = new Account
        {
            Email = email,
            Password = "hashed-password",
            IsEmailVerified = true
        };

        var loginRequest = new LoginAccountRequestDto(
            Email: email,
            Password: password
        );

        _accountRepositoryMock.Setup(x => x.GetByEmailAsync(email, true))
            .ReturnsAsync(account);

        _passwordServiceMock.Setup(x => x.Verify(account.Password, password))
            .Returns(PasswordVerificationResult.Failed);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => 
            _accessService.LoginAsync(loginRequest));
    }

    [Fact]
    public async Task LoginAsync_WithUnverifiedEmail_ThrowsUnauthorizedException()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var account = new Account
        {
            Email = email,
            Password = "hashed-password",
            IsEmailVerified = false
        };

        var loginRequest = new LoginAccountRequestDto(
            Email: email,
            Password: password
        );

        _accountRepositoryMock.Setup(x => x.GetByEmailAsync(email, true))
            .ReturnsAsync(account);

        _passwordServiceMock.Setup(x => x.Verify(account.Password, password))
            .Returns(PasswordVerificationResult.Success);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => 
            _accessService.LoginAsync(loginRequest));
    }

    [Fact]
    public async Task RefreshAsync_WithValidTokens_ReturnsNewTokens()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var oldAccessToken = "old-access-token";
        var oldRefreshToken = "old-refresh-token";
        var newAccessToken = "new-access-token";
        var newRefreshToken = "new-refresh-token";
        var role = "User";

        var account = new Account
        {
            Id = accountId,
            Role = role,
            RefreshToken = oldRefreshToken,
            RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(1)
        };

        var tokens = new Tokens
        {
            AccessToken = oldAccessToken,
            RefreshToken = oldRefreshToken
        };

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("id", accountId.ToString())
        }));

        _jwtProviderMock.Setup(x => x.GetPrincipalFromExpiredToken(oldAccessToken, _secretKeys.Access))
            .Returns(claimsPrincipal);

        _accountRepositoryMock.Setup(x => x.GetByIdAsync(accountId, true))
            .ReturnsAsync(account);

        _jwtProviderMock.Setup(x => x.GenerateToken(
                _secretKeys.Access,
                _jwtTokenOptions.AccessTokenLifetimeMinutes,
                It.IsAny<ClaimsIdentity>()))
            .Returns(newAccessToken);

        _refreshProviderMock.Setup(x => x.GenerateToken())
            .Returns(newRefreshToken);

        // Act
        var result = await _accessService.RefreshAsync(tokens);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newAccessToken, result.AccessToken);
        Assert.Equal(newRefreshToken, result.RefreshToken);
        Assert.Equal(newRefreshToken, account.RefreshToken);
        Assert.NotNull(account.RefreshTokenExpirationDate);
    }

    [Fact]
    public async Task RefreshAsync_WithInvalidRefreshToken_ThrowsBadRequestException()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var oldAccessToken = "old-access-token";
        var oldRefreshToken = "old-refresh-token";
        var differentRefreshToken = "different-refresh-token";

        var account = new Account
        {
            Id = accountId,
            RefreshToken = differentRefreshToken,
            RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(1)
        };

        var tokens = new Tokens
        {
            AccessToken = oldAccessToken,
            RefreshToken = oldRefreshToken
        };

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("id", accountId.ToString())
        }));

        _jwtProviderMock.Setup(x => x.GetPrincipalFromExpiredToken(oldAccessToken, _secretKeys.Access))
            .Returns(claimsPrincipal);

        _accountRepositoryMock.Setup(x => x.GetByIdAsync(accountId, true))
            .ReturnsAsync(account);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => 
            _accessService.RefreshAsync(tokens));
    }

    [Fact]
    public async Task RevokeAsync_WithValidTokens_RevokesTokens()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var accessToken = "access-token";
        var refreshToken = "refresh-token";

        var account = new Account
        {
            Id = accountId,
            RefreshToken = refreshToken,
            RefreshTokenExpirationDate = DateTime.UtcNow.AddDays(1)
        };

        var tokens = new Tokens
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("id", accountId.ToString())
        }));

        _jwtProviderMock.Setup(x => x.GetPrincipalFromExpiredToken(accessToken, _secretKeys.Access))
            .Returns(claimsPrincipal);

        _accountRepositoryMock.Setup(x => x.GetByIdAsync(accountId, true))
            .ReturnsAsync(account);

        // Act
        await _accessService.RevokeAsync(tokens);

        // Assert
        Assert.Null(account.RefreshToken);
        Assert.Null(account.RefreshTokenExpirationDate);
        _accountRepositoryMock.Verify(x => x.SaveAsync(), Times.Once);
    }
} 