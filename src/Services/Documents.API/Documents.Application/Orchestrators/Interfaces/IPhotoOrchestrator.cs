using Documents.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Documents.Application.Orchestrators.Interfaces;

public interface IPhotoOrchestrator
{
	public Task CreatePhotoAsync(IFormFile file, Photo photo, string fileName);
	public Task DeletePhotoAsync(Guid id, Stream photoStream, Photo photo, string fileName);
	public Task UpdatePhotoAsync(
		Photo newPhoto,
		Photo oldPhoto,
		IFormFile newFile,
		Stream oldFile,
		string newFileName,
		string oldFileName);
}
