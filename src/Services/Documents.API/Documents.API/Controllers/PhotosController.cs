using Documents.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Documents.API.Controllers;

[ApiController]
[Route("api/photos")]
public class PhotosController : ControllerBase
{
	private readonly IPhotoService _photoService;

	public PhotosController(IPhotoService photoService)
	{
		_photoService = photoService;
	}

	[HttpPost]
	public async Task<IActionResult> Upload(IFormFile photoFile)
	{
		var photo = await _photoService.CreateAsync(photoFile);

		return Ok(photo);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> Download(Guid id)
	{
		var photo = await _photoService.GetByIdAsync(id);

		return Ok(photo);
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var photos = await _photoService.GetAllAsync();

		return Ok(photos);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _photoService.DeleteAsync(id);

		return NoContent();
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, IFormFile photoFile)
	{
		await _photoService.UpdateAsync(id, photoFile);

		return NoContent();
	}
}
