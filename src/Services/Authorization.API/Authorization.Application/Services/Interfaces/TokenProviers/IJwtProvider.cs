using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authorization.Application.Services.Interfaces.JWT;

public interface IJwtProvider
{
    public string GenerateToken(string key, int lifeTimeHours, ClaimsIdentity claims);
    public JwtSecurityToken ReadToken(string token);
    public Task<bool> VerifyAccessTokenAsync(string token, string key);
    public Task<bool> VerifyEmailTokenAsync(string token, string key, string email);
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string key);
}
