using Azure.Storage.Blobs;
using Documents.Application.Extensions;
using Documents.Application.Services.Interfaces;
using Documents.Core.BlobRepositories;
using Documents.Core.Models;
using Documents.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;

namespace Documents.Application.Services.Implementations;

public class PhotoService : IPhotoService
{
	//private readonly BlobServiceClient _blobServiceClient;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IFilenameGenerator _filenameGenerator; 
	private readonly IPhotosContainer _photosContainer;

	private const string PhotosContainerName = "photos";
	public PhotoService(
		IUnitOfWork unitOfWork,
		IFilenameGenerator filenameGenerator,
		IPhotosContainer photosContainer)
	{
		_unitOfWork = unitOfWork;
		_filenameGenerator = filenameGenerator;
		_photosContainer = photosContainer;
	}

	public async Task<IEnumerable<Photo>> GetAllAsync() => await _unitOfWork.PhotoRepository.GetAllAsync();

	public async Task<Photo> CreateAsync(IFormFile photoFile)
	{
		if (!ValidatePhoto(photoFile))
		{
			throw new BadRequestException("Invalid photo format");
		}

		var filename = _filenameGenerator.Generate(photoFile.FileName);
		var photoUri = await _photosContainer.UploadAsync(photoFile, filename);

		var url = new Uri(photoUri, filename);
		var photo = new Photo(url.ToString());

		await _unitOfWork.PhotoRepository.CreateAsync(photo);

		return photo;
	}

	public async Task DeleteAsync(Guid id)
	{
		var photo = await _unitOfWork.PhotoRepository.GetByIdAsync(id);
		PhotoNullCheck(photo, id);

		var fileName = photo.GetFilename();

		await _photosContainer.DeleteAsync(fileName);
		await _unitOfWork.PhotoRepository.DeleteAsync(id);
	}

	public async Task<Photo> GetByIdAsync(Guid id)
	{
		var photo = await _unitOfWork.PhotoRepository.GetByIdAsync(id);
		PhotoNullCheck(photo, id);
		return photo;
	}

	public async Task UpdateAsync(Guid id, IFormFile photoFile)
	{
		if (!ValidatePhoto(photoFile))
		{
			throw new BadRequestException("Invalid photo format");
		}

		var oldPhoto = await _unitOfWork.PhotoRepository.GetByIdAsync(id);
		PhotoNullCheck(oldPhoto, id);

		var oldPhotoFileName = oldPhoto.GetFilename();
		var filename = _filenameGenerator.Generate(photoFile.FileName);
		var photoUri = await _photosContainer.UpdateAsync(photoFile, oldPhotoFileName, filename);

		var url = new Uri(photoUri, filename);
		var photo = new Photo(url.ToString());
		photo.Id = id;

		await _unitOfWork.PhotoRepository.UpdateAsync(photo);
	}

	private Photo PhotoNullCheck(Photo photo, Guid id) => photo ?? throw new NotFoundException(nameof(photo), id);

	private bool ValidatePhoto(IFormFile photoFile)
	{
		string[] permittedExtensions = { ".png", ".jpg", ".webp", ",jpeg" };

		var extension = Path.GetExtension(photoFile.FileName);

		return permittedExtensions.Contains(extension);
	}
}
