using Documents.Application.Orchestrators.Interfaces;
using Documents.Application.Utility;
using Documents.Core.BlobRepositories;
using Documents.Core.Models;
using Documents.Core.Repositories;
using Microsoft.AspNetCore.Http;

namespace Documents.Application.Orchestrators.Implementations;

public class DocumentOrchestrator : IDocumentOrchestrator
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IDocumentsContainer _documentContainer;

	public DocumentOrchestrator(
		IUnitOfWork unitOfWork, 
		IDocumentsContainer documentContainer)
	{
		_unitOfWork = unitOfWork;
		_documentContainer = documentContainer;
	}

	public async Task CreateAsync(IFormFile file, Document document, string fileName)
	{
		var addToDbFunction = async () => await _unitOfWork.DocumentRepository.CreateAsync(document);
		var removeFromDbFunction = async () => await _unitOfWork.DocumentRepository.DeleteAsync(document.Id);
		var addToDbTransaction = new Transaction(addToDbFunction, removeFromDbFunction);

		var addToBlobFunction = async () => await _documentContainer.UploadAsync(file, fileName);
		var removeFromBlobFunction = async () => await _documentContainer.DeleteAsync(fileName);
		var addToBlobTransaction = new Transaction(addToBlobFunction, removeFromBlobFunction);

		var orchestration = new Orchestration([addToDbTransaction, addToBlobTransaction]);
		await orchestration.ApplyAsync();
	}

	public async Task DeleteAsync(Guid id, Stream documentStream, Document document, string fileName)
	{
		var removeFromDbFunction = async () => await _unitOfWork.DocumentRepository.DeleteAsync(id);
		var addToDbFunction = async () => await _unitOfWork.DocumentRepository.CreateAsync(document);
		var removeFromDbTransaction = new Transaction(removeFromDbFunction, addToDbFunction);

		var removeFromBlobFunction = async () => await _documentContainer.DeleteAsync(fileName);
		var addToBlobFunction = async () => await _documentContainer.UploadAsync(documentStream, fileName);
		var removeFromBlobTransaction = new Transaction(removeFromBlobFunction, addToBlobFunction);

		var orchestration = new Orchestration([removeFromDbTransaction, removeFromBlobTransaction]);
		await orchestration.ApplyAsync();
	}

	public async Task UpdateAsync(
		Document newDocument, 
		Document oldDocument,
		IFormFile newFile,
		Stream oldFile,
		string newFileName, 
		string oldFileName)
	{
		var updateInDbFunction = async () => await _unitOfWork.DocumentRepository.UpdateAsync(newDocument);
		var rollbackInDbFunction = async () => await _unitOfWork.DocumentRepository.UpdateAsync(oldDocument);
		var updateInDbTransaction = new Transaction(updateInDbFunction, rollbackInDbFunction);

		var updateInBlobFunction = async () => await _documentContainer.UpdateAsync(newFile, oldFileName, newFileName);
		var rollbackInBlobFunction = async () => await _documentContainer.UpdateAsync(oldFile, newFileName, oldFileName);
		var updateInBlobTransaction = new Transaction(updateInBlobFunction, rollbackInBlobFunction);

		var orchestration = new Orchestration([updateInDbTransaction, updateInBlobTransaction]);
		await orchestration.ApplyAsync();
	}
}
