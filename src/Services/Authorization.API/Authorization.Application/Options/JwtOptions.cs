namespace Authorization.API.Options;

public class JwtOptions
{
	public int AccessTokenLifetimeMinutes { get; set; }
	public int RefreshTokenLifetimeDays { get; set; }
	public string AccessKey { get; set; }
	public string EmailKey { get; set; }
	public string Issuer { get; set; }
}
