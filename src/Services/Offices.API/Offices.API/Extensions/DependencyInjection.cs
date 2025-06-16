using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Offices.API.Options;
using Offices.Infrastructure.Options;
using System.Text;

namespace Offices.API.Extensions;

public static class DependencyInjection
{
	private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
		services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));
	}

	private static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
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
				options.ClaimsIssuer = jwtOptions.Issuer;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateAudience = false,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
					ValidateIssuer = false,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
				};
			});

		services.AddAuthorization();
	}

	private static void ConfigureCors(this IServiceCollection services)
	{
		services.AddCors(options =>
		{
			options.AddPolicy("CorsPolicy", cors =>
			{
				cors.WithOrigins("http://localhost:4200")
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials();
			});
		});
	}

	private static void ConfigureSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Description = "Add your access token",
				Name = "Authorization",
				Scheme = "Bearer",
				Type = SecuritySchemeType.Http
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					Array.Empty<string>()
				}
			});
		});
	}

	private static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
	{
		var redisSettings = configuration.GetSection("RedisSettings").Get<RedisSettings>();

		services.AddStackExchangeRedisCache(options =>
		{
			options.Configuration = redisSettings.Server;
			options.InstanceName = redisSettings.InstanceName;
		});
	}

	public static void ConfigureApiLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddEndpointsApiExplorer();
		services.AddControllers();

		services.ConfigureOptions(configuration);
		services.ConfigureAuthentication(configuration);
		services.ConfigureCors();
		services.ConfigureSwagger();
		services.ConfigureRedis(configuration);
	}
}
