using Microsoft.Extensions.DependencyInjection;
using Profiles.Application.Services.Implementations;
using Profiles.Application.Services.Interfaces;

namespace Profiles.Application.Extensions;

public static class DependencyInjection
{
	public static void ConfigureServices(this IServiceCollection services)
	{
		services.AddScoped<IDoctorService, DoctorService>();
		services.AddScoped<IPatientService, PatientService>();
		services.AddScoped<IReceptionistService, ReceptionistService>();
		services.AddScoped<ISpecializationService, SpecializationService>();
	}
}
