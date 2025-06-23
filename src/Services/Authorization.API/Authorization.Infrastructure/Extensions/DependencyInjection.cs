using Authorization.Core.Repositories;
using Authorization.Infrastructure.Consumers;
using Authorization.Infrastructure.Repositories;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Shared.Options;
using Shared.Queues;

namespace Authorization.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static void ConfigureInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services.ConfigureRepository();
		services.ConfigureMassTransit();
		services.ConfigureHealthChecks(configuration);
	}

	private static void ConfigureRepository(this IServiceCollection services)
	{
		services.AddScoped<IAccountRepository, AccountRepository>();
	}

	private static void ConfigureMassTransit(this IServiceCollection services)
	{
		services.AddMassTransit(options =>
		{
			options.AddConsumer<FullNameConsumer>();

			options.UsingRabbitMq((context, options) =>
			{
				var rabbitmqOptions = context.GetRequiredService<IOptions<RabbitmqOptions>>().Value;

				options.Host(rabbitmqOptions.Host, rabbitmqOptions.VirtualHost, host =>
				{
					host.Username(rabbitmqOptions.Username);
					host.Password(rabbitmqOptions.Password);
				});

				options.ReceiveEndpoint(QueueNames.FullNameQueue, options =>
				{
					options.ConfigureConsumer<FullNameConsumer>(context);
				});
			});
		});
	}
	
	private static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("DataBase");

		services.AddHealthChecks()
			.AddSqlServer(connectionString)
			.AddCheck("self", () => HealthCheckResult.Healthy("Service is healthy"));
	}
}
