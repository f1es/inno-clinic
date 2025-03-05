using Microsoft.AspNetCore.Http;

namespace Documents.Core.BlobRepositories;

public interface IBlobContainer
{
	public Task UploadAsync(IFormFile file, string fileName);
	public Task UploadAsync(Stream stream, string fileName);
	public Task<Stream> DownloadAsync(string fileName);
	public Task DeleteAsync(string fileName);
	public Task UpdateAsync(IFormFile file, string oldFileName, string newFileName);
	public Task UpdateAsync(Stream stream, string oldFileName, string newFileName);
	public Uri GetUriForFile(string fileName);
}
