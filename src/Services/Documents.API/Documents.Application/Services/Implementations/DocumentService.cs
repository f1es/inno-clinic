using Azure.Storage.Blobs;
using Documents.Application.Extensions;
using Documents.Application.Services.Interfaces;
using Documents.Core.Models;
using Documents.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;

namespace Documents.Application.Services.Implementations;

public class DocumentService : IDocumentService
{
	private readonly BlobServiceClient _blobServiceClient;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IFilenameGenerator _filenameGenerator;

	private const string DocumentsContainerName = "documents";
	public DocumentService(
		BlobServiceClient blobServiceClient,
		IUnitOfWork unitOfWork,
		IFilenameGenerator filenameGenerator)
	{
		_blobServiceClient = blobServiceClient;
		_unitOfWork = unitOfWork;
		_filenameGenerator = filenameGenerator;
	}

	public async Task<IEnumerable<Document>> GetAllAsync() => await _unitOfWork.DocumentRepository.GetAllAsync();

	public async Task<Document> CreateAsync(Guid resultId, IFormFile documentFile)
	{
		if (!ValidateDocument(documentFile))
		{
			throw new BadRequestException("Invalid document format");
		}

		var containerClient = _blobServiceClient.GetBlobContainerClient(DocumentsContainerName);
		await containerClient.CreateIfNotExistsAsync(publicAccessType: Azure.Storage.Blobs.Models.PublicAccessType.Blob);

		var filename = _filenameGenerator.Generate(documentFile.FileName);
		var blobClient = containerClient.GetBlobClient(filename);

		var url = new Uri(blobClient.Uri, filename);
		var document = new Document(url.ToString(), resultId);

		await UploadFileAsync(blobClient, documentFile);
		await _unitOfWork.DocumentRepository.CreateAsync(document);

		return document;
	}

	public async Task DeleteAsync(Guid id)
	{
		var document = await _unitOfWork.DocumentRepository.GetByIdAsync(id);
		DocumentNullCheck(document, id);

		var containerClient = _blobServiceClient.GetBlobContainerClient(DocumentsContainerName);
		var fileName = document.GetFilename();

		await containerClient.DeleteBlobIfExistsAsync(fileName);
		await _unitOfWork.DocumentRepository.DeleteAsync(document);
	}

	public async Task<Document> GetByIdAsync(Guid id)
	{
		var document = await _unitOfWork.DocumentRepository.GetByIdAsync(id);
		DocumentNullCheck(document, id);
		return document;
	}

	public async Task UpdateAsync(Guid id, Guid resultId, IFormFile documentFile)
	{
		var containerClient = _blobServiceClient.GetBlobContainerClient(DocumentsContainerName);

		var oldDocument = await _unitOfWork.DocumentRepository.GetByIdAsync(id);
		DocumentNullCheck(oldDocument, id);
		var oldDocumentFilename = oldDocument.GetFilename();

		await containerClient.DeleteBlobIfExistsAsync(oldDocumentFilename);

		var filename = _filenameGenerator.Generate(documentFile.FileName);
		var blobClient = containerClient.GetBlobClient(filename);

		var url = new Uri(blobClient.Uri, filename);
		var document = new Document(url.ToString(), resultId);
		document.Id = id;

		await _unitOfWork.DocumentRepository.UpdateAsync(document);

		await UploadFileAsync(blobClient, documentFile);
	}

	private async Task DeletePhotoFromContainerAsync(BlobContainerClient containerClient, string fileName)
	{
		var blobClient = containerClient.GetBlobClient(fileName);
		await blobClient.DeleteAsync();
	}

	private Document DocumentNullCheck(Document document, Guid id) => document ?? throw new NotFoundException(nameof(document), id);

	private async Task UploadFileAsync(BlobClient blobClient, IFormFile file)
	{
		using (var stream = file.OpenReadStream())
		{
			await blobClient.UploadAsync(stream, true);
		}
	}

	private bool ValidateDocument(IFormFile documentFile)
	{
		string[] permittedExtensions = { ".txt", ".pdf", ".doc", ",docx" };

		var extension = Path.GetExtension(documentFile.FileName);

		return permittedExtensions.Contains(extension);
	}
}
