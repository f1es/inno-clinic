using Documents.Application.Orchestrators.Interfaces;
using Documents.Application.Utility;
using Documents.Core.BlobRepositories;
using Documents.Core.Models;
using Documents.Core.Repositories;
using Microsoft.AspNetCore.Http;

namespace Documents.Application.Orchestrators.Implementations;

public class PhotoOrchestrator : IPhotoOrchestrator
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IPhotosContainer _photosContainer;

	public PhotoOrchestrator(
		IUnitOfWork unitOfWork,
		IPhotosContainer photosContainer)
	{
		_unitOfWork = unitOfWork;
		_photosContainer = photosContainer;
	}

	public async Task CreatePhotoAsync(IFormFile file, Photo photo, string fileName)
	{
		var addToDbFunction = async () => await _unitOfWork.PhotoRepository.CreateAsync(photo);
		var removeFromDbFunction = async () => await _unitOfWork.PhotoRepository.DeleteAsync(photo.Id);
		var addToDbTransaction = new Transaction(addToDbFunction, removeFromDbFunction);

		var addToBlobFunction = async () => await _photosContainer.UploadAsync(file, fileName);
		var removeFromBlobFunction = async () => await _photosContainer.DeleteAsync(fileName);
		var addToBlobTransaction = new Transaction(addToBlobFunction, removeFromBlobFunction);

		var orchestration = new Orchestration([addToDbTransaction, addToBlobTransaction]);
		await orchestration.ApplyAsync();
	}

	public async Task DeletePhotoAsync(Guid id, Stream photoStream, Photo photo, string fileName)
	{
		var removeFromDbFunction = async () => await _unitOfWork.PhotoRepository.DeleteAsync(id);
		var addToDbFunction = async () => await _unitOfWork.PhotoRepository.CreateAsync(photo);
		var removeFromDbTransaction = new Transaction(removeFromDbFunction, addToDbFunction);

		var removeFromBlobFunction = async () => await _photosContainer.DeleteAsync(fileName);
		var addToBlobFunction = async () => await _photosContainer.UploadAsync(photoStream, fileName);
		var removeFromBlobTransaction = new Transaction(removeFromBlobFunction, addToBlobFunction);

		var orchestration = new Orchestration([removeFromDbTransaction, removeFromBlobTransaction]);
		await orchestration.ApplyAsync();
	}

	public async Task UpdatePhotoAsync(
		Photo newPhoto,
		Photo oldPhoto,
		IFormFile newFile,
		Stream oldFile,
		string newFileName,
		string oldFileName)
	{
		var updateInDbFunction = async () => await _unitOfWork.PhotoRepository.UpdateAsync(newPhoto);
		var rollbackInDbFunction = async () => await _unitOfWork.PhotoRepository.UpdateAsync(oldPhoto);
		var updateInDbTransaction = new Transaction(updateInDbFunction, rollbackInDbFunction);

		var updateInBlobFunction = async () => await _photosContainer.UpdateAsync(newFile, oldFileName, newFileName);
		var rollbackInBlobFunction = async () => await _photosContainer.UpdateAsync(oldFile, newFileName, oldFileName);
		var updateInBlobTransaction = new Transaction(updateInBlobFunction, rollbackInBlobFunction);

		var orchestration = new Orchestration([updateInDbTransaction, updateInBlobTransaction]);
		await orchestration.ApplyAsync();
	}
}
