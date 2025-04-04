using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Services.Interfaces;

namespace Profiles.API.Controllers;

[ApiController]
[Route("api/unprotected/patients")]
[AllowAnonymous]
public class UnprotectedPatientsController : ControllerBase
{
	private readonly IPatientService _patientService;

	public UnprotectedPatientsController(IPatientService patientService)
	{
		_patientService = patientService;
	}

	/// <summary>
	/// Get patient method
	/// </summary>
	/// <param name="id">Patient's uniqie identifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}", Name = "UnprotectedGetPatient")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(Guid id)
	{
		var patient = await _patientService.GetByIdAsync(id);

		return Ok(patient);
	}
}
