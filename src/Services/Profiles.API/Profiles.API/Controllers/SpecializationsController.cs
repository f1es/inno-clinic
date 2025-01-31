using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Services.Interfaces;
using Profiles.Core.Dtos.Request;

namespace Profiles.API.Controllers;

[ApiController]
[Route("api/specializations")]
public class SpecializationsController : ControllerBase
{
	private readonly ISpecializationService _specializationService;

	public SpecializationsController(ISpecializationService specializationService)
	{
		_specializationService = specializationService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var specializations = await _specializationService.GetAllAsync();

		return Ok(specializations);
	}

	[HttpGet("{id:guid}", Name = "GetSpecialization")]
	public async Task<IActionResult> Get(Guid id)
	{
		var specialization = await _specializationService.GetByIdAsync(id);

		return Ok(specialization);
	}

	[HttpPost]
	public async Task<IActionResult> Create(SpecializationRequestDto specializationRequestDto)
	{
		var specialization = await _specializationService.CreateAsync(specializationRequestDto);

		return CreatedAtRoute("GetSpecialization", new { id = specialization.Id }, specialization);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _specializationService.DeleteAsync(id);

		return NoContent();
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, SpecializationRequestDto specializationRequestDto)
	{
		await _specializationService.UpdateAsync(id, specializationRequestDto);

		return NoContent();
	}
}
