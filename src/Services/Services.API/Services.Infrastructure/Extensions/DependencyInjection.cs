using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Services.Core.Notifiers;
using Services.Core.Repositories;
using Services.Infrastructure.Context;
using Services.Infrastructure.Notifiers;
using Services.Infrastructure.Options;
using Services.Infrastructure.Repositories;

namespace Services.Infrastructure.Extensions;

public static class DependencyInjection
{
	private static void ConfigureRepositories(this IServiceCollection services) =>
		services.AddScoped<IUnitOfWork, UnitOfWork>();

	private static void ConfigureDbContext(this IServiceCollection services, string connectionString) =>
		services.AddDbContext<ServicesDbContext>(options =>
		{
			options.UseSqlServer(connectionString);
		});

	private static void ConfigureMassTransit(this IServiceCollection services)
	{
		services.AddMassTransit(config =>
		{
			config.UsingRabbitMq((context, config) =>
			{
				var rabbitmqOptions = context.GetRequiredService<IOptions<RabbitmqOptions>>().Value;

				config.Host(rabbitmqOptions.Host, rabbitmqOptions.VirtualHost, host =>
				{
					host.Username(rabbitmqOptions.Username);
					host.Password(rabbitmqOptions.Password);
				});
			});
		});

		services.AddScoped<IDeleteServiceNotifier, DeleteServiceNotifier>();
	}

	public static void ConfigureInfrastructure(this IServiceCollection services)
	{
		var connectionStrings = services.BuildServiceProvider().GetRequiredService<IOptions<ConnectionStrings>>().Value;

		services.ConfigureDbContext(connectionStrings.DefaultConnection);
		services.ConfigureRepositories();
		services.ConfigureMassTransit();
	}
}
