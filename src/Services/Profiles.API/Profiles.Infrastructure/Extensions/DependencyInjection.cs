using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Profiles.Core.Publishers;
using Profiles.Core.Repositories;
using Profiles.Infrastructure.Options;
using Profiles.Infrastructure.Publishers;
using Profiles.Infrastructure.Repositories;

namespace Profiles.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static void ConfigureRepositories(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
	}

	public static void ConfigureMassTransit(this IServiceCollection services)
	{
		services.AddMassTransit(options =>
		{
			options.UsingRabbitMq((context, options) =>
			{
				var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

				options.Host(rabbitMqOptions.Host, rabbitMqOptions.VirtualHost, host =>
				{
					host.Username(rabbitMqOptions.Username);
					host.Password(rabbitMqOptions.Password);
				});
			});
		});

		services.AddScoped<IFullNamePublisher, FullNamePublisher>();
	}
}
