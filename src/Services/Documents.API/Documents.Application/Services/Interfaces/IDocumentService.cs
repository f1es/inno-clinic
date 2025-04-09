using Documents.Application.Dtos.Response;
using Microsoft.AspNetCore.Http;

namespace Documents.Application.Services.Interfaces;

public interface IDocumentService
{
	public Task<DocumentResponseDto> CreateAsync(Guid resultId, IFormFile documentFile);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Guid id, Guid resultId, IFormFile documentFile);
	public Task<Uri> GetByIdAsync(Guid id);
	public Task<IEnumerable<DocumentResponseDto>> GetAllAsync();
}
