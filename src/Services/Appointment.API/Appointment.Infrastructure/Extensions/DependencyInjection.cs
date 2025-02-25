using Appointment.Core.Repositories;
using Appointment.Infrastructure.Consumers;
using Appointment.Infrastructure.Repositories;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Appointment.Infrastructure.Extensions;

public static class DependencyInjection
{
	private static void ConfigureRepositories(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
	}

	public static void ConfigureInfrastructureLayer(this IServiceCollection services)
	private static void ConfigureMassTransit(this IServiceCollection services)
	{
		services.AddMassTransit(config =>
		{
			config.AddConsumer<DeleteServiceConsumer>();
			config.UsingRabbitMq((context, config) =>
			{
				config.Host("localhost", "/", host =>
				{
					host.Username("guest");
					host.Password("guest");
				});

				config.ConfigureEndpoints(context);
			});
		});
	}
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.ConfigureMassTransit();
	}
}
