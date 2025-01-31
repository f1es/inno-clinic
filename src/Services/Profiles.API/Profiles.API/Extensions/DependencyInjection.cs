using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Profiles.Infrastructure.Context;

namespace Profiles.API.Extensions;

public static class DependencyInjection
{
	public static void ConfigureDbContext(this IServiceCollection services, WebApplicationBuilder builder)
	{
		services.AddDbContext<ProfilesDbContext>(options =>
		{
			options.UseSqlServer(builder.Configuration.GetConnectionString("MsSql"));
		});
	}

	public static void ConfigureSwaggerGen(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
			options.MapType<TimeOnly>(() => new OpenApiSchema { Type = "string", Format = "time", Pattern = "00:00:00" });
		});
	}
}
