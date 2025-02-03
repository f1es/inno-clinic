using Documents.Infrastructure.Options;

namespace Documents.API.Extensions;

public static class DependencyInjection
{
	public static void ConfigureMongoOptions(this IServiceCollection services, WebApplicationBuilder builder)
	{
		services.Configure<MongoDbOptions>(builder.Configuration.GetSection("MongoDbOptions"));
	}
}
