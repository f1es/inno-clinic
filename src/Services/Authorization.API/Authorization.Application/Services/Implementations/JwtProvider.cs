using Authorization.Application.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Authorization.Application.Services.Implementations;

public class JwtProvider : IJwtProvider
{
	public string GenerateToken()
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes("SomeSecretKey1111");
		var symmetricKey = new SymmetricSecurityKey(key);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Expires = DateTime.UtcNow.AddHours(3),
			SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}
}
