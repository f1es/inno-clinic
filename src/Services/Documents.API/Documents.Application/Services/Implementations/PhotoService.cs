using Documents.Application.Extensions;
using Documents.Application.Orchestrators.Interfaces;
using Documents.Application.Services.Interfaces;
using Documents.Core.BlobRepositories;
using Documents.Core.Models;
using Documents.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;

namespace Documents.Application.Services.Implementations;

public class PhotoService : IPhotoService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IFilenameGenerator _filenameGenerator; 
	private readonly IPhotosContainer _photosContainer;
	private readonly IPhotoOrchestrator _photoOrchestrator;

	public PhotoService(
		IUnitOfWork unitOfWork,
		IFilenameGenerator filenameGenerator,
		IPhotosContainer photosContainer,
		IPhotoOrchestrator photoOrchestrator)
	{
		_unitOfWork = unitOfWork;
		_filenameGenerator = filenameGenerator;
		_photosContainer = photosContainer;
		_photoOrchestrator = photoOrchestrator;
	}

	public async Task<IEnumerable<Photo>> GetAllAsync() => await _unitOfWork.PhotoRepository.GetAllAsync();

	public async Task<Photo> CreateAsync(IFormFile photoFile)
	{
		if (!ValidatePhoto(photoFile))
		{
			throw new BadRequestException("Invalid photo format");
		}

		var fileName = _filenameGenerator.Generate(photoFile.FileName);
		var uri = _photosContainer.GetUriForFile(fileName);
		var photo = new Photo(uri.ToString());

		await _photoOrchestrator.CreatePhotoAsync(photoFile, photo, fileName);

		return photo;
	}

	public async Task DeleteAsync(Guid id)
	{
		var photo = await _unitOfWork.PhotoRepository.GetByIdAsync(id);
		PhotoNullCheck(photo, id);

		var fileName = photo.GetFilename();
		var photoStream = await _photosContainer.DownloadAsync(fileName);
		if (photoStream == null)
		{
			throw new NotFoundException(nameof(photoStream), fileName);
		}

		await _photoOrchestrator.DeletePhotoAsync(id, photoStream, photo, fileName);
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

		var oldFileName = oldPhoto.GetFilename();
		var oldPhotoStream = await _photosContainer.DownloadAsync(oldFileName);
		if (oldPhotoStream == null)
		{
			throw new NotFoundException(nameof(oldPhotoStream), oldFileName);
		}

		var newFileName = _filenameGenerator.Generate(photoFile.FileName);
		var uri = _photosContainer.GetUriForFile(newFileName);

		var newPhoto = new Photo(uri.ToString());
		newPhoto.Id = id;

		await _photoOrchestrator.UpdatePhotoAsync(newPhoto, oldPhoto, photoFile, oldPhotoStream, newFileName, oldFileName);
	}

	private Photo PhotoNullCheck(Photo photo, Guid id) => photo ?? throw new NotFoundException(nameof(photo), id);

	private bool ValidatePhoto(IFormFile photoFile)
	{
		string[] permittedExtensions = { ".png", ".jpg", ".webp", ",jpeg" };

		var extension = Path.GetExtension(photoFile.FileName);

		return permittedExtensions.Contains(extension);
	}
}
