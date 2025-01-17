using Microsoft.EntityFrameworkCore;
using Offices.Infrastructure.Context;

namespace Offices.API.Extensions;

public static class DependencyInjection
{
	public static void ConfigureDbContext(this IServiceCollection services, WebApplicationBuilder builder)
	{
		services.AddDbContext<OfficesDbContext>(options =>
		{
			options.UseSqlServer(builder.Configuration.GetConnectionString("connectionString"));
		});
	}
}
