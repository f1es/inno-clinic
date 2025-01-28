using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authorization.Application.Services.Interfaces;

public interface IJwtProvider
{
	public string GenerateToken(string key, int lifeTime, ClaimsIdentity claims);
	public JwtSecurityToken ReadToken(string token);
	public Task<bool> VerifyAccessTokenAsync(string token, string key);
	public Task<bool> VerifyEmailTokenAsync(string token, string key, string email);
	public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string key);
}
