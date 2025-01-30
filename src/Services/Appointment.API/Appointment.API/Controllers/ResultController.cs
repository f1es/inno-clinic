using Appointment.Application.Services.Interfaces;
using Appointment.Core.Dto.Request;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Appointment.API.Controllers;

[ApiController]
[Route("api/results")]
public class ResultController : ControllerBase
{
	private readonly IResultService _resultService;

	public ResultController(IResultService resultService)
	{
		_resultService = resultService;
	}

	/// <summary>
	/// Get all results method
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAll()
	{
		var results = await _resultService.GetAllAsync();

		return Ok(results);
	}

	/// <summary>
	/// Get result by id method
	/// </summary>
	/// <param name="id">Result's unique ientifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}", Name = "GetResult")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(Guid id)
	{
		var result = await _resultService.GetByIdAsync(id);

		return Ok(result);
	}

	/// <summary>
	/// Create result method
	/// </summary>
	/// <param name="resultRequestDto">Result's requets data transfer object</param>
	/// <returns></returns>
	[HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Create(ResultRequestDto resultRequestDto)
	{
		var result = await _resultService.CreateAsync(resultRequestDto);

		return CreatedAtRoute("GetResult", new { id = result.Id }, result);
	}

	/// <summary>
	/// Delete result method
	/// </summary>
	/// <param name="id">Result's unique ientifier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _resultService.DeleteAsync(id);

		return NoContent();
	}

	/// <summary>
	/// Update result method
	/// </summary>
	/// <param name="id">Result's unique ientifier</param>
	/// <param name="resultRequestDto">Result's requets data transfer object</param>
	/// <returns></returns>
	[HttpPut("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, ResultRequestDto resultRequestDto)
	{
		await _resultService.UpdateAsync(id, resultRequestDto);

		return NoContent();
	}

	/// <summary>
	/// Download result method
	/// </summary>
	/// <param name="id">Result's unique ientifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}/download")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Download(Guid id)
	{
		var result = await _resultService.GetForDownloadAsync(id);

		return File(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result)), "text/plain", fileDownloadName: "result.txt");
	}
}