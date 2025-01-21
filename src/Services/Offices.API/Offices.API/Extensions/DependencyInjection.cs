using Microsoft.EntityFrameworkCore;
using Offices.Infrastructure.Context;
using Offices.Infrastructure.Options;

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
	public static void ConfigureMongoOptions(this IServiceCollection services, WebApplicationBuilder builder)
	{
		services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
	}
}
