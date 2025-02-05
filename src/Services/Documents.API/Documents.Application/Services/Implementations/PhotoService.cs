using Azure.Storage.Blobs;
using Documents.Application.Extensions;
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
	private readonly IFilenameGenerator _filenameGenerator; 

	private const string PhotosContainerName = "photos";
	public PhotoService(
		BlobServiceClient blobServiceClient,
		IUnitOfWork unitOfWork,
		IFilenameGenerator filenameGenerator)
	{
		_blobServiceClient = blobServiceClient;
		_unitOfWork = unitOfWork;
		_filenameGenerator = filenameGenerator;
	}

	public async Task<IEnumerable<Photo>> GetAllAsync() => await _unitOfWork.PhotoRepository.GetAllAsync();

	public async Task<Photo> CreateAsync(IFormFile photoFile)
	{
		if (!ValidatePhoto(photoFile))
		{
			throw new BadRequestException("Invalid photo format");
		}

		var containerClient = _blobServiceClient.GetBlobContainerClient(PhotosContainerName);
		await containerClient.CreateIfNotExistsAsync(publicAccessType: Azure.Storage.Blobs.Models.PublicAccessType.Blob);

		var filename = _filenameGenerator.Generate(photoFile.FileName);
		var blobClient = containerClient.GetBlobClient(filename);

		var url = new Uri(blobClient.Uri, filename);
		var photo = new Photo(url.ToString());

		await UploadFileAsync(blobClient, photoFile);
		await _unitOfWork.PhotoRepository.CreateAsync(photo);

		return photo;
	}

	public async Task DeleteAsync(Guid id)
	{
		var photo = await _unitOfWork.PhotoRepository.GetByIdAsync(id);
		PhotoNullCheck(photo, id);

		var containerClient = _blobServiceClient.GetBlobContainerClient(PhotosContainerName);
		var fileName = photo.GetFilename();

		await containerClient.DeleteBlobIfExistsAsync(fileName);
		await _unitOfWork.PhotoRepository.DeleteAsync(photo);
	}

	public async Task<Photo> GetByIdAsync(Guid id)
	{
		var photo = await _unitOfWork.PhotoRepository.GetByIdAsync(id);
		PhotoNullCheck(photo, id);
		return photo;
	}

	public async Task UpdateAsync(Guid id, IFormFile photoFile)
	{
		var containerClient = _blobServiceClient.GetBlobContainerClient(PhotosContainerName);

		var oldPhoto = await _unitOfWork.PhotoRepository.GetByIdAsync(id);
		PhotoNullCheck(oldPhoto, id);
		var oldPhotoFileName = oldPhoto.GetFilename();

		await containerClient.DeleteBlobIfExistsAsync(oldPhotoFileName);

		var filename = _filenameGenerator.Generate(photoFile.FileName);
		var blobClient = containerClient.GetBlobClient(filename);

		var url = new Uri(blobClient.Uri, filename);
		var photo = new Photo(url.ToString());
		photo.Id = id;

		await _unitOfWork.PhotoRepository.UpdateAsync(photo);

		await UploadFileAsync(blobClient, photoFile);
	}

	private Photo PhotoNullCheck(Photo photo, Guid id) => photo ?? throw new NotFoundException(nameof(photo), id);

	private async Task UploadFileAsync(BlobClient blobClient, IFormFile file)
	{
		using (var stream = file.OpenReadStream())
		{
			await blobClient.UploadAsync(stream, true);
		}
	}

	private bool ValidatePhoto(IFormFile photoFile)
	{
		string[] permittedExtensions = { ".png", ".jpg", ".webp", ",jpeg" };

		var extension = Path.GetExtension(photoFile.FileName);

		return permittedExtensions.Contains(extension);
	}
}
