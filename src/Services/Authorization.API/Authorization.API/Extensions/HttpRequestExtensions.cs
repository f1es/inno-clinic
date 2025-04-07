using Authorization.Application.Utility;
using Shared.Exceptions;

namespace Authorization.API.Extensions;

public static class HttpRequestExtensions
{
	public static Tokens GetAccessAndRefreshTokens(this HttpRequest httpRequest)
	{
		var accessToken = httpRequest.Cookies["sec"];
		var refreshToken = httpRequest.Cookies["ref"];

		return new Tokens
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken
		};
	}

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
