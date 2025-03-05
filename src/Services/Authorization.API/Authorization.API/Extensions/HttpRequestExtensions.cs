using Authorization.Application.Utility;

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
}
