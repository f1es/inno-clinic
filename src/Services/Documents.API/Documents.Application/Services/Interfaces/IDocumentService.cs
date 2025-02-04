using Documents.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Documents.Application.Services.Interfaces;

public interface IDocumentService
{
	public Task<Document> CreateAsync(IFormFile documentFile);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Guid id, IFormFile documentFile);
	public Task<Document> GetByIdAsync(Guid id);
	public Task<IEnumerable<Document>> GetAllAsync();
}
