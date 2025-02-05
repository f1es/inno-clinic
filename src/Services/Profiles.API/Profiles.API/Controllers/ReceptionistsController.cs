using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Services.Interfaces;
using Profiles.Core.Dtos.Request;
using Profiles.Core.Parameters;

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

	/// <summary>
	/// Get receptionists method
	/// </summary>
	/// <param name="requestParameters">Request parameters for collections</param>
	/// <returns></returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAll([FromQuery] RequestParameters requestParameters)
	{
		var receptionists = await _receptionistService.GetAllAsync(requestParameters);

		return Ok(receptionists);
	}

	/// <summary>
	/// Get receptionists method
	/// </summary>
	/// <param name="id">Receptionist's unique identifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}", Name = "GetReceptionist")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(Guid id)
	{
		var receptionist = await _receptionistService.GetByIdAsync(id);

		return Ok(receptionist);
	}

	/// <summary>
	/// Create receptionist method
	/// </summary>
	/// <param name="receptionistRequestDto">Receptionist's request data transfer object</param>
	/// <returns></returns>
	[HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Create(ReceptionistRequestDto receptionistRequestDto)
	{
		var receptionist = await _receptionistService.CreateAsync(receptionistRequestDto);

		return CreatedAtRoute("GetReceptionist", new { id = receptionist.Id }, receptionist);
	}

	/// <summary>
	/// Update receptionist method
	/// </summary>
	/// <param name="id">Receptionist's unique identifier</param>
	/// <param name="receptionistRequestDto">Receptionist's request data transfer object</param>
	/// <returns></returns>
	[HttpPut("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, ReceptionistRequestDto receptionistRequestDto)
	{
		await _receptionistService.UpdateAsync(id, receptionistRequestDto);

		return NoContent();
	}

	/// <summary>
	/// Delete receptionist method
	/// </summary>
	/// <param name="id">Receptionist's unique identifier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _receptionistService.DeleteAsync(id);

		return NoContent();
	}
}
