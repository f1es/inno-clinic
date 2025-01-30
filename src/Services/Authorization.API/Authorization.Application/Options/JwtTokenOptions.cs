namespace Authorization.Application.Options;

public class JwtTokenOptions
{
	public int AccessTokenLifetime { get; set; }
	public int RefreshTokenLifetime { get; set; }
}
