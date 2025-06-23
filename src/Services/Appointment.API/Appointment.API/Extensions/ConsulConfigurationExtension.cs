using Appointment.API.Options;
using Winton.Extensions.Configuration.Consul;

namespace Appointment.API.Extensions;

public static class ConsulConfigurationExtension
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