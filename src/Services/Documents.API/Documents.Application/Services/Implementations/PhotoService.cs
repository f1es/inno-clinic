using Azure.Storage.Blobs;
using Documents.Application.Services.Interfaces;
using Documents.Core.Models;
using Documents.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;

namespace Documents.Application.Services.Implementations;

public class PhotoService : IPhotoService
{
	private readonly BlobServiceClient _blobServiceClient;
	private readonly IUnitOfWork _unitOfWork;

	private const string PhotosContainerName = "photos";
	public PhotoService(
		BlobServiceClient blobServiceClient,
		IUnitOfWork unitOfWork)
	{
		_blobServiceClient = blobServiceClient;
		_unitOfWork = unitOfWork;
	}

	public async Task<IEnumerable<Photo>> GetAllAsync() => await _unitOfWork.PhotoRepository.GetAllAsync();

	public async Task<Photo> CreateAsync(IFormFile photoFile)
	{
		var containerClient = _blobServiceClient.GetBlobContainerClient(PhotosContainerName);
		await containerClient.CreateIfNotExistsAsync();

		var blobClient = containerClient.GetBlobClient(photoFile.FileName);

		var url = new Uri(blobClient.Uri, photoFile.FileName);
		var photo = new Photo(url.ToString());

		await _unitOfWork.PhotoRepository.CreateAsync(photo);

		using (var stream = photoFile.OpenReadStream())
		{
			await blobClient.UploadAsync(stream, true);
		}

		return photo;
	}

	public async Task DeleteAsync(Guid id)
	{
		var containerClient = _blobServiceClient.GetBlobContainerClient(PhotosContainerName);

		var photo = await _unitOfWork.PhotoRepository.GetByIdAsync(id);

		if (photo == null)
		{
			throw new NotFoundException(nameof(photo), id);
		}

		var fileName = photo.Url.Split('/').Last();
		var blobClient = containerClient.GetBlobClient(fileName);

		await blobClient.DeleteAsync();
		await _unitOfWork.PhotoRepository.DeleteAsync(photo);
	}

	public async Task<Photo> GetByIdAsync(Guid id)
	{
		var photo = await _unitOfWork.PhotoRepository.GetByIdAsync(id);

		if (photo == null)
		{
			throw new NotFoundException(nameof(photo), id);
		}

		return photo;
	}

	public async Task UpdateAsync(Guid id, IFormFile photoFile)
	{
		var containerClient = _blobServiceClient.GetBlobContainerClient(PhotosContainerName);
		await containerClient.CreateIfNotExistsAsync();

		var oldPhoto = await _unitOfWork.PhotoRepository.GetByIdAsync(id);
		if (oldPhoto == null)
		{
			throw new NotFoundException("photo", id);
		}
		var oldPhotoFileName = oldPhoto.Url.Split('/').Last();
		if (oldPhotoFileName != photoFile.FileName)
		{
			var oldPhotoBlobClient = containerClient.GetBlobClient(oldPhotoFileName);
			await oldPhotoBlobClient.DeleteAsync();
		}

		var blobClient = containerClient.GetBlobClient(photoFile.FileName);

		var url = new Uri(blobClient.Uri, photoFile.FileName);
		var photo = new Photo(url.ToString());
		photo.Id = id;

		await _unitOfWork.PhotoRepository.UpdateAsync(photo);

		using (var stream = photoFile.OpenReadStream())
		{
			await blobClient.UploadAsync(stream, true);
		}
	}
}
