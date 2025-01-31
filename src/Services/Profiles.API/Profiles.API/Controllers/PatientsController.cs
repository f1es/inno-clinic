using Microsoft.AspNetCore.Mvc;
using Profiles.Application.Services.Interfaces;
using Profiles.Core.Dtos.Request;

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

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var patients = await _patientService.GetAllAsync();

		return Ok(patients);
	}

	[HttpGet("{id:guid}", Name = "GetPatient")]
	public async Task<IActionResult> Get(Guid id)
	{
		var patient = await _patientService.GetByIdAsync(id);

		return Ok(patient);
	}

	[HttpPost]
	public async Task<IActionResult> Create(PatientRequestDto patientRequestDto)
	{
		var patient = await _patientService.CreateAsync(patientRequestDto);

		return CreatedAtRoute("GetPatient", new { id = patient.Id }, patient);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _patientService.DeleteAsync(id);

		return NoContent();
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, PatientRequestDto patientRequestDto)
	{
		await _patientService.UpdateAsync(id, patientRequestDto);

		return NoContent();
	}
}
