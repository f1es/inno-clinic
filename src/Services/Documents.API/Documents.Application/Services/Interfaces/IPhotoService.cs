using Documents.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Documents.Application.Services.Interfaces;

public interface IPhotoService
{
	public Task<Photo> CreateAsync(IFormFile photoFile);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Guid id, IFormFile photoFile);
	public Task<Photo> GetByIdAsync(Guid id);
	public Task<IEnumerable<Photo>> GetAllAsync();
}
