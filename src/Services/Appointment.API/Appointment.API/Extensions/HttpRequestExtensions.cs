using Shared.Exceptions;

namespace Appointment.API.Extensions;

public static class HttpRequestExtensions
{
	public static string GetJwtFromHeader(this HttpRequest httpRequest)
	{
		string authHeader = httpRequest.Headers["Authorization"];

		if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
		{
			return authHeader.Substring("Bearer ".Length).Trim();
		}

		throw new UnauthorizedException("Missing access token");
	}
}
