using Documents.Core.Models;

namespace Documents.Core.Repositories;

public interface IPhotoRepository
{
	public Task<Photo> GetByIdAsync(Guid id);
	public Task<IEnumerable<Photo>> GetAllAsync();
	public Task CreateAsync(Photo photo);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Photo photo);
}
