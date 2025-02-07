using Authorization.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Authorization.API.Extensions;

public static class DependencyInjection
{
	public static void ConfigureDbContext(this IServiceCollection services, WebApplicationBuilder builder) =>
		services.AddDbContext<AuthorizationDbContext>(option =>
		{
			option.UseSqlServer(builder.Configuration.GetConnectionString("DataBase"));
		});
}
