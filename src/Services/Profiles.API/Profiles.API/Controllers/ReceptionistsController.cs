using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Services.Interfaces;
using Profiles.Core.Dtos.Request;

namespace Profiles.API.Controllers;

[ApiController]
[Route("api/receptionists")]
public class ReceptionistsController : ControllerBase
{
	private readonly IReceptionistService _receptionistService;

	public ReceptionistsController(IReceptionistService receptionistService)
	{
		_receptionistService = receptionistService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var receptionists = await _receptionistService.GetAllAsync();

		return Ok(receptionists);
	}

	[HttpGet("{id:guid}", Name = "GetReceptionist")]
	public async Task<IActionResult> Get(Guid id)
	{
		var receptionist = await _receptionistService.GetByIdAsync(id);

		return Ok(receptionist);
	}

	[HttpPost]
	public async Task<IActionResult> Create(ReceptionistRequestDto receptionistRequestDto)
	{
		var receptionist = await _receptionistService.CreateAsync(receptionistRequestDto);

		return CreatedAtRoute("GetReceptionist", new { id = receptionist.Id }, receptionist);
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, ReceptionistRequestDto receptionistRequestDto)
	{
		await _receptionistService.UpdateAsync(id, receptionistRequestDto);

		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _receptionistService.DeleteAsync(id);

		return NoContent();
	}
}
