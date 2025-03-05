namespace Documents.Core.Repositories;

public interface IUnitOfWork
{
	public IDocumentRepository DocumentRepository { get; }
	public IPhotoRepository PhotoRepository { get; }
}
