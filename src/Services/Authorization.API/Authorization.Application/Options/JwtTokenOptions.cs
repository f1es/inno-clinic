namespace Authorization.Application.Options;

public class JwtTokenOptions
{
	public int AccessTokenLifetimeMinutes { get; set; }
	public int RefreshTokenLifetimeDays { get; set; }
	public string Issuer { get; set; }
}
