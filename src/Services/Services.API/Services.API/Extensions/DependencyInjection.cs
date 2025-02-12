using Services.Infrastructure.Options;

namespace Services.API.Extensions;

public static class DependencyInjection
{
	private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
	}

	public static void ConfigureApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddControllers();
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();
		services.ConfigureOptions(configuration);
	}
}
