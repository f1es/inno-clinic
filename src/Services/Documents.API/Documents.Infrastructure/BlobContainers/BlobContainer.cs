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
		BlobServiceClient blobServiceClient)
    {
		_blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
		_blobContainerClient.CreateIfNotExists(publicAccessType: Azure.Storage.Blobs.Models.PublicAccessType.Blob);
	}
	public async Task DeleteAsync(string fileName)
	{
		await _blobContainerClient.DeleteBlobIfExistsAsync(fileName);
	}

	public async Task<Uri> UpdateAsync(IFormFile file, string oldFileName, string newFileName)
	{
		await _blobContainerClient.DeleteBlobIfExistsAsync(oldFileName);

		var blobClient = _blobContainerClient.GetBlobClient(newFileName);
		await UploadFileAsync(blobClient, file);

		return blobClient.Uri;
	}

	public async Task<Uri> UploadAsync(IFormFile file, string fileName)
	{
		var blobClient = _blobContainerClient.GetBlobClient(fileName);
		await UploadFileAsync(blobClient, file);

		return blobClient.Uri;
	}

	private async Task UploadFileAsync(BlobClient blobClient, IFormFile file)
	{
		using (var stream = file.OpenReadStream())
		{
			await blobClient.UploadAsync(stream, true);
		}
	}
}
