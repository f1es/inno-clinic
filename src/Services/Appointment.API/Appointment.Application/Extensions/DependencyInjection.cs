using Appointment.Application.Mappers.Implementations;
using Appointment.Application.Mappers.Interfaces;
using Appointment.Application.Services.Implementations;
using Appointment.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Appointment.Application.Extensions;

public static class DependencyInjection
{
	public static void ConfigureMappers(this IServiceCollection services)
	{
		services.AddScoped<IResultsMapper, ResultsMapper>();
		services.AddScoped<IAppointmentsMapper, AppointmentsMapper>();
	}

	public static void ConfigureServices(this IServiceCollection services)
	{
		services.AddScoped<IAppointmentService, AppointmentService>();
		services.AddScoped<IResultService, ResultService>();
	}
}
