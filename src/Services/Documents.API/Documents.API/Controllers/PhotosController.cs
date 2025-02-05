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

	/// <summary>
	/// Create photo method
	/// </summary>
	/// <param name="photoFile">Photo file</param>
	/// <returns></returns>
	[HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Create(IFormFile photoFile)
	{
		var photo = await _photoService.CreateAsync(photoFile);

		return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photo);
	}

	/// <summary>
	/// Get photo by id method
	/// </summary>
	/// <param name="id">Photo's unique identifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}", Name = "GetPhoto")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(Guid id)
	{
		var photo = await _photoService.GetByIdAsync(id);

		return Ok(photo);
	}

	/// <summary>
	/// Get all photos method
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAll()
	{
		var photos = await _photoService.GetAllAsync();

		return Ok(photos);
	}

	/// <summary>
	/// Delete photo method
	/// </summary>
	/// <param name="id">Photo's unique identifier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _photoService.DeleteAsync(id);

		return NoContent();
	}

	/// <summary>
	/// Update photo method
	/// </summary>
	/// <param name="id">Photo's unique identifier</param>
	/// <param name="photoFile">Photo file</param>
	/// <returns></returns>
	[HttpPut("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, IFormFile photoFile)
	{
		await _photoService.UpdateAsync(id, photoFile);

		return NoContent();
	}
}
