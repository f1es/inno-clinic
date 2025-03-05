using Documents.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Documents.Application.Orchestrators.Interfaces;

public interface IDocumentOrchestrator
{
	public Task CreateAsync(IFormFile file, Document document, string fileName);
	public Task DeleteAsync(Guid id, Stream documentStream, Document document, string fileName);
	public Task UpdateAsync(
		Document newDocument,
		Document oldDocument,
		IFormFile newFile,
		Stream oldFile,
		string newFileName,
		string oldFileName);
}
