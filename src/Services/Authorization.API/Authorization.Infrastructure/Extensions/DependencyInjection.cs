using Authorization.Core.Repositories;
using Authorization.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static void ConfigureRepository(this IServiceCollection services)
	{
		services.AddScoped<IAccountRepository, AccountRepository>();
	}
}
