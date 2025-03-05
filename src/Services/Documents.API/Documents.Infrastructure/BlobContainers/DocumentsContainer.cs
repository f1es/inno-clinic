using Azure.Storage.Blobs;
using Documents.Core.BlobRepositories;
using Documents.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Documents.Infrastructure.BlobRepositories;

public class DocumentsContainer : BlobContainer, IDocumentsContainer
{
	private const string DocumentsContainerName = "documents";
	public DocumentsContainer(
        BlobServiceClient blobServiceClient,
        IOptions<DomainsOptions> domains)
        : base(
            DocumentsContainerName, 
            domains.Value.BlobDomain, 
            blobServiceClient)
    {
        
    }
}
