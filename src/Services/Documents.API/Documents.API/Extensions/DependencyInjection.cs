using Documents.Infrastructure.Options;
using Microsoft.Extensions.Azure;

namespace Documents.API.Extensions;

public static class DependencyInjection
{
	public static void ConfigureMongoOptions(this IServiceCollection services, WebApplicationBuilder builder)
	{
		services.Configure<MongoDbOptions>(builder.Configuration.GetSection("MongoDbOptions"));
	}

	public static void ConfigureAzureServices(this IServiceCollection services, WebApplicationBuilder builder)
	{
		services.AddAzureClients(options =>
		{
			options.AddBlobServiceClient(builder.Configuration.GetConnectionString("BlobStorage"));
		});
	}
}
