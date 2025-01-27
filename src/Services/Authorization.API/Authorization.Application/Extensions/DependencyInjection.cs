using Authorization.Application.Configuration;
using Authorization.Application.Options;
using Authorization.Application.Services.Implementations;
using Authorization.Application.Services.Interfaces;
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
	}

	public static void ConfigureValidators(this IServiceCollection services) =>
		services.AddValidatorsFromAssemblyContaining(typeof(RegisterAccountRequestDtoValidator));

	public static void ConfigureKeys(this IServiceCollection services, WebApplicationBuilder builder) =>
		services.Configure<SecretKeys>(builder.Configuration.GetSection("SecretKeys"));

	public static void ConfigureEmailOptions(this IServiceCollection services, WebApplicationBuilder builder) =>
		services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailConfiguration"));
	
}
