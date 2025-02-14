using Microsoft.AspNetCore.Http;

namespace Documents.Core.BlobRepositories;

public interface IBlobContainer
{
	public Task<Uri> UploadAsync(IFormFile file, string fileName);
	public Task DeleteAsync(string fileName);
	public Task<Uri> UpdateAsync(IFormFile file, string oldFileName, string newFileName);
}
