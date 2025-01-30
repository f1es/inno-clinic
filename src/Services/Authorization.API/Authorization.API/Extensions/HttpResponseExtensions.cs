using Authorization.Application.Utility;

namespace Authorization.API.Extensions;

public static class HttpResponseExtensions
{
	public static void AddAccessAndRefreshTokensToCookie(
		this HttpResponse response, 
		Tokens tokens, 
		bool isSession = false)
	{
		CookieOptions cookieOptions = new CookieOptions
		{
			HttpOnly = true,
		};

		cookieOptions.Expires = isSession ? null : DateTime.UtcNow.AddDays(31);

		response.Cookies.Append("sec", tokens.AccessToken, cookieOptions);
		response.Cookies.Append("ref", tokens.RefreshToken, cookieOptions);
	}
}
