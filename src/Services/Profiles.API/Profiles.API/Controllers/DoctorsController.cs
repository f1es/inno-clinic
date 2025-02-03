using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Services.Interfaces;
using Profiles.Core.Dtos.Request;
using Profiles.Core.Parameters;

namespace Profiles.API.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorsController : ControllerBase
{
	private readonly IDoctorService _doctorService;

	public DoctorsController(IDoctorService doctorService)
	{
		_doctorService = doctorService;
	}

	/// <summary>
	/// Get doctors method
	/// </summary>
	/// <param name="requestParameters">Request parameters for collections</param>
	/// <returns></returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAll([FromQuery] RequestParameters requestParameters)
	{
		var doctors = await _doctorService.GetAllAsync(requestParameters);

		return Ok(doctors);
	}

	/// <summary>
	/// Get doctor method
	/// </summary>
	/// <param name="id">Doctor's uniqie identifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}", Name = "GetDoctor")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(Guid id)
	{
		var doctor = await _doctorService.GetByIdAsync(id);

		return Ok(doctor);
	}

	/// <summary>
	/// Create doctor method
	/// </summary>
	/// <param name="doctorRequestDto">Doctor's request data transfer object</param>
	/// <returns></returns>
	[HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Create(DoctorRequestDto doctorRequestDto)
	{
		var doctor = await _doctorService.CreateAsync(doctorRequestDto);

		return CreatedAtRoute("GetDoctor", new { id = doctor.Id }, doctor);
	}

	/// <summary>
	/// Delete doctor method
	/// </summary>
	/// <param name="id">Doctor's uniqie identifier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _doctorService.DeleteAsync(id);
		
		return NoContent();
	}

	/// <summary>
	/// Update doctor method
	/// </summary>
	/// <param name="id">Doctor's uniqie identifier</param>
	/// <param name="doctorRequestDto">Doctor's request data transfer object</param>
	/// <returns></returns>
	[HttpPut("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, DoctorRequestDto doctorRequestDto)
	{
		await _doctorService.UpdateAsync(id, doctorRequestDto);

		return NoContent();
	}
}
