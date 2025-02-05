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
	}
}
