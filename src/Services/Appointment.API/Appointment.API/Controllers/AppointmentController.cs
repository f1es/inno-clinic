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

	/// <summary>
	/// Get all appointments method
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAll()
	{
		var appointments = await _appointmentService.GetAllAsync();

		return Ok(appointments);
	}

	/// <summary>
	/// Get appointment by id method
	/// </summary>
	/// <param name="id">Appointment's unique identier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}", Name = "GetAppointment")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(Guid id)
	{
		var appointment = await _appointmentService.GetByIdAsync(id);

		return Ok(appointment);
	}

	/// <summary>
	/// Create appointment method
	/// </summary>
	/// <param name="appointmentRequestDto">Request appointment data transfer object</param>
	/// <returns></returns>
	[HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Create(AppointmentRequestDto appointmentRequestDto)
	{
		var appoinment = await _appointmentService.CreateAsync(appointmentRequestDto);

		return CreatedAtRoute("GetAppointment", new { id = appoinment.Id }, appoinment);
	}

	/// <summary>
	/// Update appointment method
	/// </summary>
	/// <param name="id">Appointment's unique identier</param>
	/// <param name="appointmentRequestDto">Request appointment data transfer object</param>
	/// <returns></returns>
	[HttpPut("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, AppointmentRequestDto appointmentRequestDto)
	{
		await _appointmentService.UpdateAsync(id, appointmentRequestDto);

		return NoContent();
	}

	/// <summary>
	/// Delete appointment method
	/// </summary>
	/// <param name="id">Appointment's unique identier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _appointmentService.DeleteAsync(id);

		return NoContent();
	}
}