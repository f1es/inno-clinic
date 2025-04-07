using Appointment.Application.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace Appointment.Application.Services.Implementations;

public class JwtService : IJwtService
{
	private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

	public JwtService(JwtSecurityTokenHandler jwtSecurityTokenHandler)
	{
		_jwtSecurityTokenHandler = jwtSecurityTokenHandler;
	}

	public Guid GetAccountId(string jwt)
	{
		var token = _jwtSecurityTokenHandler.ReadJwtToken(jwt);
		var accountId = token.Claims.FirstOrDefault(x => x.Type == "id").Value;
		return Guid.Parse(accountId);
	}
}
