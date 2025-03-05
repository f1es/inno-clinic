using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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
	}

	public static void ConfigureApplicationLayer(this IServiceCollection services)
	{
		services.ConfigureMappers();
		services.ConfigureServices();
	}
}
