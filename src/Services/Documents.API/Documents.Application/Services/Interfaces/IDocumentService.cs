using Documents.Core.Dtos.Request;
using Documents.Core.Dtos.Response;

namespace Documents.Application.Services.Interfaces;

public interface IDocumentService
{
	public Task<FileResponseDto> CreateAsync(DocumentRequestDto documentRequestDto);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Guid id, DocumentRequestDto documentRequestDto);
	public Task<FileResponseDto> GetByIdAsync(Guid id);
	public Task<IEnumerable<FileResponseDto>> GetAllAsync();
}
