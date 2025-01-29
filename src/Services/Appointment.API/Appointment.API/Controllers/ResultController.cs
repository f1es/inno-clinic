using Appointment.Application.Services.Interfaces;
using Appointment.Core.Dto.Request;
using Microsoft.AspNetCore.Mvc;

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

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var results = await _resultService.GetAllAsync();

		return Ok(results);
	}

	[HttpGet("{id:guid}", Name = "GetResult")]
	public async Task<IActionResult> Get(Guid id)
	{
		var result = await _resultService.GetByIdAsync(id);

		return Ok(result);
	}

	[HttpPost]
	public async Task<IActionResult> Create(ResultRequestDto resultRequestDto)
	{
		var result = await _resultService.CreateAsync(resultRequestDto);

		return CreatedAtRoute("GetResult", new { id = result.Id }, result);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _resultService.DeleteAsync(id);

		return NoContent();
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, ResultRequestDto resultRequestDto)
	{
		await _resultService.UpdateAsync(id, resultRequestDto);

		return NoContent();
	}
}
