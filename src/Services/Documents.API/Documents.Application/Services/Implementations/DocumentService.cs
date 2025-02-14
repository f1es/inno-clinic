using Documents.Application.Extensions;
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

	private const string DocumentsContainerName = "documents";
	public DocumentService(
		IUnitOfWork unitOfWork,
		IFilenameGenerator filenameGenerator,
		IDocumentsContainer documentsContainer)
	{
		_unitOfWork = unitOfWork;
		_filenameGenerator = filenameGenerator;
		_documentsContainer = documentsContainer;
	}

	public async Task<IEnumerable<Document>> GetAllAsync() => await _unitOfWork.DocumentRepository.GetAllAsync();

	public async Task<Document> CreateAsync(Guid resultId, IFormFile documentFile)
	{
		if (!ValidateDocument(documentFile))
		{
			throw new BadRequestException("Invalid document format");
		}

		var filename = _filenameGenerator.Generate(documentFile.FileName);
		var fileUri = await _documentsContainer.UploadAsync(documentFile, filename);

		var url = new Uri(fileUri, filename);
		var document = new Document(url.ToString(), resultId);
		await _unitOfWork.DocumentRepository.CreateAsync(document);

		return document;
	}

	public async Task DeleteAsync(Guid id)
	{
		var document = await _unitOfWork.DocumentRepository.GetByIdAsync(id);
		DocumentNullCheck(document, id);

		var fileName = document.GetFilename();
		await _documentsContainer.DeleteAsync(fileName);
		await _unitOfWork.DocumentRepository.DeleteAsync(id);
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

		var oldDocumentFilename = oldDocument.GetFilename();
		var filename = _filenameGenerator.Generate(documentFile.FileName);
		var documentUri = await _documentsContainer.UpdateAsync(documentFile, oldDocumentFilename, filename);

		var url = new Uri(documentUri, filename);
		var document = new Document(url.ToString(), resultId);
		document.Id = id;

		await _unitOfWork.DocumentRepository.UpdateAsync(document);
	}

	private Document DocumentNullCheck(Document document, Guid id) => document ?? throw new NotFoundException(nameof(document), id);

	private bool ValidateDocument(IFormFile documentFile)
	{
		string[] permittedExtensions = { ".txt", ".pdf", ".doc", ",docx" };

		var extension = Path.GetExtension(documentFile.FileName);

		return permittedExtensions.Contains(extension);
	}
}
