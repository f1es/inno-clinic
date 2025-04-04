namespace Authorization.Infrastructure.Options;

public class RabbitmqOptions
{
	public string Username { get; set; }
	public string Password { get; set; }
	public string Host { get; set; }
	public string VirtualHost { get; set; }
}