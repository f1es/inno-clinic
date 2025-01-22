using Microsoft.Extensions.DependencyInjection;
using Services.Core.Repositories;
using Services.Infrastructure.Repositories;

namespace Services.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static void ConfigureRepositories(this IServiceCollection services) =>
		services.AddScoped<IUnitOfWork, UnitOfWork>();
}
