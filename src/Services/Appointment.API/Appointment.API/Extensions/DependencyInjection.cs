using Appointment.Infrastructure.Context;
using Appointment.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Text.Json.Serialization;
using Appointment.API.Options;
using Appointment.Application.Options;

namespace Appointment.API.Extensions;

public static class DependencyInjection
{
	public static void ConfigureApiLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services.ConfigureDbContext(configuration);
		services.ConfigureControllers();
		services.ConfigureSwaggerGen();
		services.AddEndpointsApiExplorer();
		services.ConfigureOptions(configuration);
		services.ConfigureAuthentication(configuration);
		services.ConfigureCors();
	}

	private static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<AppointmentDbContext>(options =>
		{
			options.UseNpgsql(configuration.GetConnectionString("NpsSql"));
		});
	}

	private static void ConfigureControllers(this IServiceCollection services)
	{
		services.AddControllers()
			.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			});
	}

	private static void ConfigureSwaggerGen(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
			options.MapType<TimeOnly>(() => new OpenApiSchema { Type = "string", Format = "time", Pattern = "00:00:00" });

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

	private static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<RabbitmqOptions>(configuration.GetSection("RabbitmqOptions"));
		services.Configure<ServicesEndpoints>(configuration.GetSection("ServicesEndpoints"));
		services.Configure<PatientsEndpoints>(configuration.GetSection("PatientsEndpoints"));
		services.Configure<AccountsEndpoints>(configuration.GetSection("AccountsEndpoints"));
		services.Configure<EmailCredentials>(configuration.GetSection("EmailCredentials"));
		services.Configure<CronOptions>(configuration.GetSection("CronOptions"));
	}
}
