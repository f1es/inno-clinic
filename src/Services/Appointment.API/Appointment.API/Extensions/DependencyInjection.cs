using Appointment.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace Appointment.API.Extensions;

public static class DependencyInjection
{
	public static void ConfigureDbContext(this IServiceCollection services, WebApplicationBuilder builder)
	{
		services.AddDbContext<AppointmentDbContext>(options =>
		{
			options.UseNpgsql(builder.Configuration.GetConnectionString("NpsSql"));
		});
	}
}
