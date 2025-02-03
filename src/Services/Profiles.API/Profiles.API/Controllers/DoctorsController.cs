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

	[HttpGet]
	public async Task<IActionResult> GetAll([FromQuery] RequestParameters requestParameters)
	{
		var doctors = await _doctorService.GetAllAsync(requestParameters);

		return Ok(doctors);
	}

	[HttpGet("{id:guid}", Name = "GetDoctor")]
	public async Task<IActionResult> Get(Guid id)
	{
		var doctor = await _doctorService.GetByIdAsync(id);

		return Ok(doctor);
	}

	[HttpPost]
	public async Task<IActionResult> Create(DoctorRequestDto doctorRequestDto)
	{
		var doctor = await _doctorService.CreateAsync(doctorRequestDto);

		return CreatedAtRoute("GetDoctor", new { id = doctor.Id }, doctor);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _doctorService.DeleteAsync(id);
		
		return NoContent();
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, DoctorRequestDto doctorRequestDto)
	{
		await _doctorService.UpdateAsync(id, doctorRequestDto);

		return NoContent();
	}
}
