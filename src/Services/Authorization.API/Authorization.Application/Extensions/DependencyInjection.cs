using Authorization.Application.Services.Implementations;
using Authorization.Application.Services.Interfaces;
using Authorization.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.Application.Extensions;

public static class DependencyInjection
{
	public static void ConfigureServices(this IServiceCollection services)
	{
		services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
		services.AddScoped<IPasswordService, PasswordService>();
		services.AddScoped<IJwtProvider, JwtProvider>();
		services.AddScoped<IAccountService, AccountService>();
	}
}
