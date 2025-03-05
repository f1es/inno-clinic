using Documents.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Documents.Application.Orchestrators.Interfaces;

public interface IPhotoOrchestrator
{
	public Task CreateAsync(IFormFile file, Photo photo, string fileName);
	public Task DeleteAsync(Guid id, Stream photoStream, Photo photo, string fileName);
	public Task UpdateAsync(
		Photo newPhoto,
		Photo oldPhoto,
		IFormFile newFile,
		Stream oldFile,
		string newFileName,
		string oldFileName);
}
