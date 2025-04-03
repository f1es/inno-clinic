using Appointment.Application.Jobs;
using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Appointment.Application.Extensions;

public static class DependencyInjection
{
	private static void ConfigureMappers(this IServiceCollection services)
	{
		services.AddScoped<IResultsMapper, ResultsMapper>();
		services.AddScoped<IAppointmentsMapper, AppointmentsMapper>();
	}

	private static void ConfigureServices(this IServiceCollection services)
	{
		services.AddScoped<IAppointmentService, AppointmentService>();
		services.AddScoped<IResultService, ResultService>();
		services.AddScoped<ITimeSlotService, TimeSlotService>();
		services.AddScoped<INotifyService, NotifyService>();
	}

	private static void ConfigureQuartzScheduler(this IServiceCollection services)
	{
		services.AddQuartz(options =>
		{
			var jobIdentity = "appointments-notification-job";

			options.AddJob<AppointmentNotificationJob>(options =>
			{
				options.WithIdentity(jobIdentity);
			});

			options.AddTrigger(options =>
			{
				options.ForJob(jobIdentity);
				options.WithCronSchedule("0 16 * * * ?");
			});
		});
		services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
	}

	public static void ConfigureApplicationLayer(this IServiceCollection services)
	{
		services.ConfigureMappers();
		services.ConfigureServices();
		services.ConfigureQuartzScheduler();
	}
}
