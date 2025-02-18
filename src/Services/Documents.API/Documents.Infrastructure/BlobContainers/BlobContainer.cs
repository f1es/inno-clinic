using Azure.Storage.Blobs;
using Documents.Core.BlobRepositories;
using Microsoft.AspNetCore.Http;

namespace Documents.Infrastructure.BlobRepositories;

public abstract class BlobContainer : IBlobContainer
{
	protected readonly BlobContainerClient _blobContainerClient;
	protected readonly string _domain;
    protected BlobContainer(
		string containerName,
		string domain,
		BlobServiceClient blobServiceClient)
    {
		_blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
		_domain = domain;
		_blobContainerClient.CreateIfNotExists(publicAccessType: Azure.Storage.Blobs.Models.PublicAccessType.Blob);
	}

	public async Task DeleteAsync(string fileName)
	{
		await _blobContainerClient.DeleteBlobIfExistsAsync(fileName);
	}

	public async Task UpdateAsync(IFormFile file, string oldFileName, string newFileName)
	{
		using (var stream = file.OpenReadStream())
		{
			await UpdateAsync(stream, oldFileName, newFileName);
		}
	}

	public async Task UploadAsync(IFormFile file, string fileName)
	{
		using (var stream = file.OpenReadStream())
		{
			await UploadAsync(stream, fileName);
		}
	}

	public async Task UpdateAsync(Stream stream, string oldFileName, string newFileName)
	{
		await _blobContainerClient.DeleteBlobIfExistsAsync(oldFileName);

		var blobClient = _blobContainerClient.GetBlobClient(newFileName);
		await blobClient.UploadAsync(stream, true);
	}

	public async Task UploadAsync(Stream stream, string fileName)
	{
		var blobClient = _blobContainerClient.GetBlobClient(fileName);
		await blobClient.UploadAsync(stream);
	}

	public Uri GetUriForFile(string fileName)
	{
		var blobClient = _blobContainerClient.GetBlobClient(fileName);

		var uriBuilder = new UriBuilder(blobClient.Uri);
		uriBuilder.Host = _domain;
		return uriBuilder.Uri;
	}
}
