using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Services.Interfaces;
using Profiles.Core.Dtos.Request;
using Profiles.Core.Parameters;

namespace Profiles.API.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
	private readonly IPatientService _patientService;

	public PatientsController(IPatientService patientService)
	{
		_patientService = patientService;
	}

	/// <summary>
	/// Get patients method
	/// </summary>
	/// <param name="requestParameters">Request parameters for collections</param>
	/// <returns></returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAll([FromQuery] RequestParameters requestParameters)
	{
		var patients = await _patientService.GetAllAsync(requestParameters);

		return Ok(patients);
	}

	/// <summary>
	/// Get patient method
	/// </summary>
	/// <param name="id">Patient's uniqie identifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}", Name = "GetPatient")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(Guid id)
	{
		var patient = await _patientService.GetByIdAsync(id);

		return Ok(patient);
	}

	/// <summary>
	/// Create patient method
	/// </summary>
	/// <param name="patientRequestDto">Patient's request data transfer object</param>
	/// <returns></returns>
	[HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Create(PatientRequestDto patientRequestDto)
	{
		var patient = await _patientService.CreateAsync(patientRequestDto);

		return CreatedAtRoute("GetPatient", new { id = patient.Id }, patient);
	}

	/// <summary>
	/// Delete patient method
	/// </summary>
	/// <param name="id">Patient's uniqie identifier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _patientService.DeleteAsync(id);

		return NoContent();
	}

	/// <summary>
	/// Update patient method
	/// </summary>
	/// <param name="id">Patient's uniqie identifier</param>
	/// <param name="patientRequestDto">Patient's request data transfer object</param>
	/// <returns></returns>
	[HttpPut("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, PatientRequestDto patientRequestDto)
	{
		await _patientService.UpdateAsync(id, patientRequestDto);

		return NoContent();
	}
}
