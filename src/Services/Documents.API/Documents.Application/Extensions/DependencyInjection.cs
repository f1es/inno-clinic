using Documents.Application.Orchestrators.Implementations;
using Documents.Application.Orchestrators.Interfaces;
using Documents.Application.Services.Implementations;
using Documents.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Documents.Application.Extensions;

public static class DependencyInjection
{
	public static void ConfigureServices(this IServiceCollection services)
	{
		services.AddScoped<IPhotoService, PhotoService>();
		services.AddSingleton<IFilenameGenerator, FilenameGenerator>();
		services.AddScoped<IDocumentService, DocumentService>();
		services.AddScoped<IPhotoOrchestrator, PhotoOrchestrator>();
		services.AddScoped<IDocumentOrchestrator, DocumentOrchestrator>();
	}
}
