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
}
