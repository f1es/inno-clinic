using Documents.Core.Repositories;
using Documents.Infrastructure.Context;

namespace Documents.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly Lazy<IDocumentRepository> _documentRepository;
	private readonly Lazy<IPhotoRepository> _photoRepository;

    public UnitOfWork(DocumentsDbContext context)
    {
        _documentRepository = new Lazy<IDocumentRepository>(() =>
        new DocumentRepository(context));

        _photoRepository = new Lazy<IPhotoRepository>(() =>
        new PhotoRepository(context));
    }

    public IDocumentRepository DocumentRepository => _documentRepository.Value;

	public IPhotoRepository PhotoRepository => _photoRepository.Value;
}
