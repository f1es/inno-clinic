using Appointment.Core.GrpcClients;
using Appointment.Core.Repositories;
using Appointment.Core.RequestClients;
using Appointment.Infrastructure.Consumers;
using Appointment.Infrastructure.GrpcClients;
using Appointment.Infrastructure.Options;
using Appointment.Infrastructure.Repositories;
using Appointment.Infrastructure.RequestClients;
using GrpcServices;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Appointment.Infrastructure.Extensions;

public static class DependencyInjection
{
	private static void ConfigureRepositories(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
	}

	private static void ConfigureMassTransit(this IServiceCollection services)
	{
		services.AddMassTransit(config =>
		{
			config.AddConsumer<DeleteServiceConsumer>();
			config.UsingRabbitMq((context, config) =>
			{
				var rabbitmqOptions = context.GetRequiredService<IOptions<RabbitmqOptions>>().Value;

				config.Host(rabbitmqOptions.Host, rabbitmqOptions.VirtualHost, host =>
				{
					host.Username(rabbitmqOptions.Username);
					host.Password(rabbitmqOptions.Password);
				});

				config.ConfigureEndpoints(context);
			});
		});
	}

	private static void ConfigureGrpcClient(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddGrpcClient<ServiceGrpcServiceProto.ServiceGrpcServiceProtoClient>(options =>
		{
			options.Address = new Uri(configuration.GetConnectionString("ServicesService"));
		});

		services.AddScoped<IServiceGrpcClient, ServiceGrpcClient>();
	}

	private static void ConfigureRequestClients(this IServiceCollection services)
	{
		services.AddScoped<HttpClient>();
		services.AddScoped<IServicesRequestClient, ServicesRequestClient>();
	}

	public static void ConfigureInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.ConfigureMassTransit();
		services.ConfigureGrpcClient(configuration);
		services.ConfigureRequestClients();
	}
}
