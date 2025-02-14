using Authorization.Application.Configuration;
using Authorization.Application.Options;
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

	public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<JwtTokenOptions>(configuration.GetSection("JwtTokenOptions"));
		services.Configure<EmailOptions>(configuration.GetSection("EmailConfiguration"));
		services.Configure<SecretKeys>(configuration.GetSection("SecretKeys"));
		services.Configure<EndpointsOptions>(configuration.GetSection("Endpoints"));
	}
}
