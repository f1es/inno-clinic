using Offices.Infrastructure.Options;

namespace Offices.API.Extensions;

public static class DependencyInjection
{
	public static void ConfigureOptions(this IServiceCollection services, WebApplicationBuilder builder)
	{
		services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
	}
}
