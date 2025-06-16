using Appointment.Core.Email;
using Appointment.Core.Notifiers;
using Appointment.Core.Repositories;
using Appointment.Core.RequestClients;
using Appointment.Infrastructure.Consumers;
using Appointment.Infrastructure.Email;
using Appointment.Infrastructure.Hubs;
using Appointment.Infrastructure.Notifiers;
using Appointment.Infrastructure.Options;
using Appointment.Infrastructure.Repositories;
using Appointment.Infrastructure.RequestClients;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;

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

	private static void ConfigureRequestClients(this IServiceCollection services)
	{
		services.AddScoped<IServicesRequestClient, ServicesRequestClient>();
		services.AddScoped<IPatientsRequestClient, PatientsRequestClient>();
		services.AddScoped<IAccountRequestClient, AccountRequestClient>();
		services.AddScoped<IDocumentRequestClient, DocumentsRequestClient>();
	}

	private static void ConfigureSmtpClient(this IServiceCollection services)
	{
		services.AddScoped<IEmailSender, EmailSender>();
	}

	private static void ConfigureSignalR(this IServiceCollection services)
	{
		services.AddSignalR();

		services.AddSingleton<IUserIdProvider, SignalRUserIdProvider>();
		services.AddScoped<IDoctorNotificationSender, DoctorNotificationSender>();
	}

	public static void ConfigureInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.ConfigureMassTransit();
		services.ConfigureRequestClients();
		services.ConfigureSmtpClient();
		services.ConfigureSignalR();
	}
}
