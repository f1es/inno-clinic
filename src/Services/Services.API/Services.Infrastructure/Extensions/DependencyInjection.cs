using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Services.Core.Repositories;
using Services.Infrastructure.Context;
using Services.Infrastructure.Options;
using Services.Infrastructure.Repositories;

namespace Services.Infrastructure.Extensions;

public static class DependencyInjection
{
	private static void ConfigureRepositories(this IServiceCollection services) =>
		services.AddScoped<IUnitOfWork, UnitOfWork>();

	private static void ConfigureDbContext(this IServiceCollection services, string connectionString) =>
		services.AddDbContext<ServicesDbContext>(options =>
		{
			options.UseSqlServer(connectionString);
		});

	public static void ConfigureInfrastructure(
		this IServiceCollection services)
	{
		var connectionStrings = services.BuildServiceProvider().GetRequiredService<IOptions<ConnectionStrings>>().Value;

		services.ConfigureDbContext(connectionStrings.DefaultConnection);
		services.ConfigureRepositories();
	}
}
