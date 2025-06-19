using Authorization.API.Options;
using Winton.Extensions.Configuration.Consul;

namespace Authorization.API.Builders;

public static class ConsulConfigurationBuilder
{
	public static IConsulConfigurationSource AddConsulConfiguration(this IConsulConfigurationSource source, IConfiguration configuration)
	{
		var consulOptions = configuration.GetSection("ConsulOptions").Get<ConsulOptions>();
		if (consulOptions == null)
		{
			throw new ArgumentNullException(nameof(consulOptions), "Consul options cannot be null");
		}

		source.ConsulConfigurationOptions = cco => cco.Address = new Uri(consulOptions.Server);
		source.ReloadOnChange = true;
		source.Optional = false;

		return source;
	}
}
