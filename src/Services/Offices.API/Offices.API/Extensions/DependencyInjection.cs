using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Offices.API.Options;
using Offices.Infrastructure.Options;
using System.Text;

namespace Offices.API.Extensions;

public static class DependencyInjection
{
	public static void ConfigureOptions(this IServiceCollection services, WebApplicationBuilder builder)
	{
		services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
		services.Configure<Keys>(builder.Configuration.GetSection("Keys"));
	}

	public static void ConfigureAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
	{
		var key = builder.Configuration.GetSection("Keys").GetRequiredSection("Access");

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme =	JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
			.AddJwtBearer(options =>
			{
				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						if (context.Request.Cookies.ContainsKey("sec"))
						{
							context.Token = context.Request.Cookies["sec"];
						}

						return Task.CompletedTask;
					}
				};
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key.Value))
				};
			});

		services.AddAuthorization();
	}
}
