using Authorization.Core.Repositories;
using Authorization.Infrastructure.Consumers;
using Authorization.Infrastructure.Options;
using Authorization.Infrastructure.Repositories;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Queues;

namespace Authorization.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static void ConfigureInfrastructureLayer(this IServiceCollection services)
	{
		services.ConfigureRepository();
		services.ConfigureMassTransit();
	}

	private static void ConfigureRepository(this IServiceCollection services)
	{
		services.AddScoped<IAccountRepository, AccountRepository>();
	}

	private static void ConfigureMassTransit(this IServiceCollection services)
	{
		services.AddMassTransit(options =>
		{
			options.AddConsumer<DeleteFullNameConsumer>();
			options.AddConsumer<UpdateFullNameConsumer>();

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
					options.ConfigureConsumer<DeleteFullNameConsumer>(context);
					options.ConfigureConsumer<UpdateFullNameConsumer>(context);
				});
			});
		});
	}
}
