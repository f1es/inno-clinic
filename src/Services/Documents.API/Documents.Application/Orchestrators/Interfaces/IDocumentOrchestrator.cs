using Documents.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Documents.Application.Orchestrators.Interfaces;

public interface IDocumentOrchestrator
{
	public Task CreateDocumentAsync(IFormFile file, Document document, string fileName);
	public Task DeleteDocumentAsync(Guid id, Stream documentStream, Document document, string fileName);
	public Task UpdateDocumentAsync(
		Document newDocument,
		Document oldDocument,
		IFormFile newFile,
		Stream oldFile,
		string newFileName,
		string oldFileName);
}
