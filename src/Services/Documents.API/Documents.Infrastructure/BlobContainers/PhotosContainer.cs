using Azure.Storage.Blobs;
using Documents.Core.BlobRepositories;
using Documents.Infrastructure.BlobRepositories;

namespace Documents.Infrastructure.BlobContainers;

public class PhotosContainer : BlobContainer, IPhotosContainer
{
	private const string PhotosContainerName = "photos";
	public PhotosContainer(BlobServiceClient blobServiceClient)
        : base(PhotosContainerName, blobServiceClient)
    {
        
    }
}
