using Authorization.Application.Configuration;
using Authorization.Application.Options;
using Authorization.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Authorization.API.Extensions;

public static class DependencyInjection
{
	public static void ConfigureApiLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services.ConfigureDbContext(configuration);
		services.ConfigureOptions(configuration);
		services.AddControllers();
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();
	}

	private static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration) =>
		services.AddDbContext<AuthorizationDbContext>(option =>
		{
			option.UseSqlServer(configuration.GetConnectionString("DataBase"));
		});

	private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<JwtTokenOptions>(configuration.GetSection("JwtTokenOptions"));
		services.Configure<EmailOptions>(configuration.GetSection("EmailConfiguration"));
		services.Configure<SecretKeys>(configuration.GetSection("SecretKeys"));
		services.Configure<EndpointsOptions>(configuration.GetSection("Endpoints"));
	}
}
