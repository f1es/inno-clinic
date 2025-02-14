using Authorization.Application.Configuration;
using Authorization.Application.Options;
using Authorization.Application.Services.Implementations.Accounts;
using Authorization.Application.Services.Implementations.Authentication;
using Authorization.Application.Services.Implementations.Email;
using Authorization.Application.Services.Implementations.JWT;
using Authorization.Application.Services.Implementations.TokenProviders;
using Authorization.Application.Services.Interfaces.Accounts;
using Authorization.Application.Services.Interfaces.Authentication;
using Authorization.Application.Services.Interfaces.Email;
using Authorization.Application.Services.Interfaces.JWT;
using Authorization.Application.Services.Interfaces.TokenProviers;
using Authorization.Application.Validators;
using Authorization.Core.Models;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace Authorization.Application.Extensions;

public static class DependencyInjection
{
	public static void ConfigureServices(this IServiceCollection services)
	{
		services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
		services.AddScoped<IPasswordService, PasswordService>();
		services.AddScoped<IJwtProvider, JwtProvider>();
		services.AddScoped<IAccountService, AccountService>();
		services.AddScoped<IAccessService, AccessService>();
		services.AddScoped<IEmailSender, EmailSender>();
		services.AddScoped<JwtSecurityTokenHandler>();
		services.AddScoped<IRefreshProvider, RefreshProvider>();
		services.AddScoped<IRegistrationService, RegistrationService>();
		services.AddScoped<IEmailVerificationService, EmailVerificationService>();
	}

	public static void ConfigureValidators(this IServiceCollection services) =>
		services.AddValidatorsFromAssemblyContaining(typeof(RegisterAccountRequestDtoValidator));
}
