using Microsoft.EntityFrameworkCore;
using Services.Infrastructure.Context;

namespace Services.API.Extensions;

public static class DependencyInjection
{
	public static void ConfigureDbContext(this IServiceCollection services, WebApplicationBuilder builder) =>
		services.AddDbContext<ServicesDbContext>(options =>
		{
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
		});
}
