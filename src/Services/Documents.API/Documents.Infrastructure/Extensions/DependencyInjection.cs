using Documents.Core.BlobRepositories;
using Documents.Core.Repositories;
using Documents.Infrastructure.BlobContainers;
using Documents.Infrastructure.BlobRepositories;
using Documents.Infrastructure.Context;
using Documents.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Documents.Infrastructure.Extensions;

public static class DependencyInjection
{
	public static void ConfigureRepositories(this IServiceCollection services)
	{
		services.AddSingleton<DocumentsDbContext>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IPhotosContainer, PhotosContainer>();
		services.AddScoped<IDocumentsContainer, DocumentsContainer>();
	}
}
