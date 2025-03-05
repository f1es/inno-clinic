using Documents.Application.Dtos.Response;
using Microsoft.AspNetCore.Http;

namespace Documents.Application.Services.Interfaces;

public interface IPhotoService
{
	public Task<FileResponseDto> CreateAsync(IFormFile photoFile);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Guid id, IFormFile photoFile);
	public Task<Uri> GetByIdAsync(Guid id);
	public Task<IEnumerable<FileResponseDto>> GetAllAsync();
}
