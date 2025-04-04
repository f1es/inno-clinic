using Appointment.Application.Jobs;
using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Options;
using Appointment.Application.Services.Implementations;
using Appointment.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

	private static void ConfigureQuartzScheduler(this IServiceCollection services, IConfiguration configuration)
	{
		var cronOptions = configuration.GetSection("CronOptions").Get<CronOptions>();

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
				options.WithCronSchedule(cronOptions.AppointmentsReminderCron);
			});
		});
		services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
	}

	public static void ConfigureApplicationLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services.ConfigureMappers();
		services.ConfigureServices();
		services.ConfigureQuartzScheduler(configuration);
	}
}
