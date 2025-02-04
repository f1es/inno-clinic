using Documents.Core.Models;

namespace Documents.Core.Repositories;

public interface IDocumentRepository
{
	public Task<Document> GetByIdAsync(Guid id);
	public Task<IEnumerable<Document>> GetAllAsync();
	public Task CreateAsync(Document document);
	public Task DeleteAsync(Document document);
	public Task UpdateAsync(Document document);
}
