using Appointment.Application.Services.Interfaces;
using Appointment.Core.Dto.Request;
using Microsoft.AspNetCore.Mvc;

namespace Appointment.API.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentController : ControllerBase
{
	private readonly IAppointmentService _appointmentService;

	public AppointmentController(IAppointmentService appointmentService)
	{
		_appointmentService = appointmentService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var appointments = await _appointmentService.GetAllAsync();

		return Ok(appointments);
	}

	[HttpGet("{id:guid}", Name = "GetAppointment")]
	public async Task<IActionResult> Get(Guid id)
	{
		var appointment = await _appointmentService.GetByIdAsync(id);

		return Ok(appointment);
	}

	[HttpPost]
	public async Task<IActionResult> Create(AppointmentRequestDto appointmentRequestDto)
	{
		var appoinment = await _appointmentService.CreateAsync(appointmentRequestDto);

		return CreatedAtRoute("GetAppointment", new { id = appoinment.Id }, appoinment);
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, AppointmentRequestDto appointmentRequestDto)
	{
		await _appointmentService.UpdateAsync(id, appointmentRequestDto);

		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _appointmentService.DeleteAsync(id);

		return NoContent();
	}

}
