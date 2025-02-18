using Documents.Application.Extensions;
using Documents.Application.Orchestrators.Interfaces;
using Documents.Application.Services.Interfaces;
using Documents.Core.BlobRepositories;
using Documents.Core.Models;
using Documents.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;

namespace Documents.Application.Services.Implementations;

public class DocumentService : IDocumentService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IFilenameGenerator _filenameGenerator;
	private readonly IDocumentsContainer _documentsContainer;
	private readonly IDocumentOrchestrator _documentOrchestrator;

	public DocumentService(
		IUnitOfWork unitOfWork,
		IFilenameGenerator filenameGenerator,
		IDocumentsContainer documentsContainer,
		IDocumentOrchestrator documentOrchestrator)
	{
		_unitOfWork = unitOfWork;
		_filenameGenerator = filenameGenerator;
		_documentsContainer = documentsContainer;
		_documentOrchestrator = documentOrchestrator;
	}

	public async Task<IEnumerable<Document>> GetAllAsync() => await _unitOfWork.DocumentRepository.GetAllAsync();

	public async Task<Document> CreateAsync(Guid resultId, IFormFile documentFile)
	{
		if (!ValidateDocument(documentFile))
		{
			throw new BadRequestException("Invalid document format");
		}

		var fileName = _filenameGenerator.Generate(documentFile.FileName);
		var uri = _documentsContainer.GetUriForFile(fileName);
		var document = new Document(uri.ToString(), resultId);

		await _documentOrchestrator.CreateAsync(documentFile, document, fileName);

		return document;
	}

	public async Task DeleteAsync(Guid id)
	{
		var document = await _unitOfWork.DocumentRepository.GetByIdAsync(id);
		DocumentNullCheck(document, id);

		var fileName = document.GetFilename();
		var documentStream = await _documentsContainer.DownloadAsync(fileName);
		if (documentStream == null)
		{
			throw new NotFoundException(nameof(documentStream), fileName);
		}

		await _documentOrchestrator.DeleteAsync(id, documentStream, document, fileName);
	}

	public async Task<Document> GetByIdAsync(Guid id)
	{
		var document = await _unitOfWork.DocumentRepository.GetByIdAsync(id);
		DocumentNullCheck(document, id);
		return document;
	}

	public async Task UpdateAsync(Guid id, Guid resultId, IFormFile documentFile)
	{
		if (!ValidateDocument(documentFile))
		{
			throw new BadRequestException("Invalid document format");
		}

		var oldDocument = await _unitOfWork.DocumentRepository.GetByIdAsync(id);
		DocumentNullCheck(oldDocument, id);

		var oldFileName = oldDocument.GetFilename();
		var documentStream = await _documentsContainer.DownloadAsync(oldFileName);
		if (documentStream == null)
		{
			throw new NotFoundException(nameof(documentStream), oldFileName);
		}

		var newFileName = _filenameGenerator.Generate(documentFile.FileName);
		var uri = _documentsContainer.GetUriForFile(newFileName);
		var newDocument = new Document(uri.ToString(), resultId);
		newDocument.Id = id;

		await _documentOrchestrator.UpdateAsync(newDocument, oldDocument, documentFile, documentStream, newFileName, oldFileName);

	}

	private Document DocumentNullCheck(Document document, Guid id) => document ?? throw new NotFoundException(nameof(document), id);

	private bool ValidateDocument(IFormFile documentFile)
	{
		string[] permittedExtensions = { ".txt", ".pdf", ".doc", ",docx" };

		var extension = Path.GetExtension(documentFile.FileName);

		return permittedExtensions.Contains(extension);
	}
}
