using Documents.Core.Models;
using Documents.Core.Repositories;
using Documents.Infrastructure.Context;
using MongoDB.Driver;

namespace Documents.Infrastructure.Repositories;

public class DocumentRepository : IDocumentRepository
{
	private readonly DocumentsDbContext _context;

	public DocumentRepository(DocumentsDbContext context)
	{
		_context = context;
	}

	public async Task CreateAsync(Document document) => await _context.Documents.InsertOneAsync(document);

	public async Task DeleteAsync(Guid id) => await _context.Documents.DeleteOneAsync(Builders<Document>.Filter.Eq(x => x.Id, id));

	public async Task<IEnumerable<Document>> GetAllAsync() => (await _context.Documents.FindAsync(Builders<Document>.Filter.Empty)).ToList();

	public async Task<Document> GetByIdAsync(Guid id) => (await _context.Documents.FindAsync(Builders<Document>.Filter.Eq(x => x.Id, id))).FirstOrDefault();

	public async Task UpdateAsync(Document document) => await _context.Documents.ReplaceOneAsync(Builders<Document>.Filter.Eq(x => x.Id, document.Id), document);
}
