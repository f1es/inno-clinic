using Documents.Core.Dtos.Request;
using Documents.Core.Dtos.Response;

namespace Documents.Application.Services.Interfaces;

public interface IPhotoService
{
	public Task<FileResponseDto> CreateAsync(PhotoRequestDto photoRequestDto);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Guid id, PhotoRequestDto photoRequestDto);
	public Task<FileResponseDto> GetByIdAsync(Guid id);
	public Task<IEnumerable<FileResponseDto>> GetAllAsync();
}
