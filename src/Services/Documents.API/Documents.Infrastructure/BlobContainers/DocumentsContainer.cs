using Azure.Storage.Blobs;
using Documents.Core.BlobRepositories;

namespace Documents.Infrastructure.BlobRepositories;

public class DocumentsContainer : BlobContainer, IDocumentsContainer
{
	private const string DocumentsContainerName = "documents";
	public DocumentsContainer(BlobServiceClient blobServiceClient)
        : base(DocumentsContainerName, blobServiceClient)
    {
        
    }
}
