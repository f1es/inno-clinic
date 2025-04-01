namespace Offices.Infrastructure.Options;

public class RedisSettings
{
	public string Server { get; set; }
	public string InstanceName { get; set; }
	public int CacheTimeMinutes { get; set; }
}
