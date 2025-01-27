using Authorization.Application.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authorization.Application.Services.Implementations;

public class JwtProvider : IJwtProvider
{
	private readonly JwtSecurityTokenHandler _tokenHandler;
	public JwtProvider(JwtSecurityTokenHandler tokenHandler)
	{
		_tokenHandler = tokenHandler;
	}

	public string GenerateToken(string key, int lifeTime, ClaimsIdentity claims)
	{
		var symmetricKey = ReadKey(key);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = claims,
			Expires = DateTime.UtcNow.AddHours(lifeTime),
			SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature)
		};

		var token = _tokenHandler.CreateToken(tokenDescriptor);
		return _tokenHandler.WriteToken(token);
	}

	public async Task<bool> VerifyAccessTokenAsync(string token, string key) => await VerifyTokenAsync(token, key);

	public async Task<bool> VerifyEmailTokenAsync(string token, string key, string email)
	{
		var verificationResult = await VerifyTokenAsync(token, key, validateLifetime: false);

		var jwt = _tokenHandler.ReadJwtToken(token);
		var isEmailEquals = jwt.Claims.First(x => x.Type == "email").Value == email;

		return verificationResult && isEmailEquals;
	}

	public JwtSecurityToken ReadToken(string token) => _tokenHandler.ReadJwtToken(token);

	private SymmetricSecurityKey ReadKey(string key) => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

	private async Task<bool> VerifyTokenAsync(string token, string key, bool validateLifetime = true)
	{
		var symmetricKey = ReadKey(key);
		var validationParameters = new TokenValidationParameters
		{
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = validateLifetime,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = symmetricKey
		};

		var result = await _tokenHandler.ValidateTokenAsync(token, validationParameters);

		if (!result.IsValid)
		{
			return false;
		}

		return true;
	}
}
