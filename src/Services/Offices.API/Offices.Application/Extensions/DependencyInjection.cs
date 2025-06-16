using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Offices.Application.MapperProfiles;
using Offices.Application.Options;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Offices.Application.Extensions;

public static class DependencyInjection
{
	private static void ConfigureAutomapper(this IServiceCollection services)
	{
		services.AddAutoMapper(config =>
		{
			config.AddProfile<OfficeMapperProfile>();
		});
	}

	private static void ConfigureMediatr(this IServiceCollection services)
	{
		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
		});
	}

	private static void ConfigureSerilog(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSerilog();

		var elasticSearchSettings = configuration.GetSection("ElasticSearch").Get<ElasticSearchSettings>();

		var elasticSearchSinkOptions = new ElasticsearchSinkOptions(new Uri(elasticSearchSettings.Server))
		{
			AutoRegisterTemplate = true,
			AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
			IndexFormat = "offices-{0:yyyy.MM.dd}",
			MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information
		};

		Log.Logger = new LoggerConfiguration()
			.Enrich.FromLogContext()
			.WriteTo.Console()
			.WriteTo.Elasticsearch(elasticSearchSinkOptions)
			.CreateLogger();

		Log.Logger.Information("Serilog configured with ElasticSearch sink at {ElasticSearchServer}", elasticSearchSettings.Server);
	}

	public static void ConfigureApplicationLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services.ConfigureAutomapper();
		services.ConfigureMediatr();
		services.ConfigureSerilog(configuration);
	}
}
