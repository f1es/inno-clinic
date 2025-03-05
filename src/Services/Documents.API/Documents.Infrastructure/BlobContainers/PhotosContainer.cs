using Azure.Storage.Blobs;
using Documents.Core.BlobRepositories;
using Documents.Infrastructure.BlobRepositories;
using Documents.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Documents.Infrastructure.BlobContainers;

public class PhotosContainer : BlobContainer, IPhotosContainer
{
	private const string PhotosContainerName = "photos";
	public PhotosContainer(
        BlobServiceClient blobServiceClient,
        IOptions<DomainsOptions> domains)
        : base(
            PhotosContainerName,
            domains.Value.BlobDomain,
            blobServiceClient)
    {
        
    }
}
