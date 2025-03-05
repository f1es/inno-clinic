using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Services.Interfaces;
using Profiles.Core.Dtos.Request;
using Profiles.Core.Parameters;

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

	/// <summary>
	/// Get specializations method
	/// </summary>
	/// <param name="requestParameters">Request parameters for collections</param>
	/// <returns></returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAll([FromQuery] RequestParameters requestParameters)
	{
		var specializations = await _specializationService.GetAllAsync(requestParameters);
		return Ok(specializations);
	}

	/// <summary>
	/// Get specialization method
	/// </summary>
	/// <param name="id">Specialization's unique udentifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}", Name = "GetSpecialization")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(Guid id)
	{
		var specialization = await _specializationService.GetByIdAsync(id);
		return Ok(specialization);
	}

	/// <summary>
	/// Create specialization method
	/// </summary>
	/// <param name="specializationRequestDto">Specialization's request data transfer object</param>
	/// <returns></returns>
	[HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Create(SpecializationRequestDto specializationRequestDto)
	{
		var specialization = await _specializationService.CreateAsync(specializationRequestDto);
		return CreatedAtRoute("GetSpecialization", new
		{
			id = specialization.Id
		}, specialization);
	}

	/// <summary>
	/// Delete specialization method
	/// </summary>
	/// <param name="id">Specialization's unique udentifier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _specializationService.DeleteAsync(id);

		return NoContent();
	}

	/// <summary>
	/// Update specialization method
	/// </summary>
	/// <param name="id">Specialization's unique udentifier</param>
	/// <param name="specializationRequestDto">Specialization's request data transfer object</param>
	/// <returns></returns>
	[HttpPut("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, SpecializationRequestDto specializationRequestDto)
	{
		await _specializationService.UpdateAsync(id, specializationRequestDto);

		return NoContent();
	}
}
