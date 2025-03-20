using Appointment.Application.Services.Interfaces;
using Appointment.Core.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Appointment.API.Controllers;

[ApiController]
[Route("api/appointments")]
[Authorize]
public class AppointmentController : ControllerBase
{
	private readonly IAppointmentService _appointmentService;
	private readonly ITimeSlotService _timeSlotService;

	public AppointmentController(IAppointmentService appointmentService, ITimeSlotService timeSlotService)
	{
		_appointmentService = appointmentService;
		_timeSlotService = timeSlotService;
	}

	/// <summary>
	/// Get all appointments method
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
	{
		var appointments = await _appointmentService.GetAllAsync(cancellationToken);

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
	public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
	{
		var appointment = await _appointmentService.GetByIdAsync(id, cancellationToken);

		return Ok(appointment);
	}

	/// <summary>
	/// Create appointment method
	/// </summary>
	/// <param name="appointmentRequestDto">Request appointment data transfer object</param>
	/// <returns></returns>
	[HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Create(AppointmentRequestDto appointmentRequestDto, CancellationToken cancellationToken)
	{
		var appoinment = await _appointmentService.CreateAsync(appointmentRequestDto, cancellationToken);

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
	[Authorize(Roles = "doctor,receptionist")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, AppointmentRequestDto appointmentRequestDto, CancellationToken cancellationToken)
	{
		await _appointmentService.UpdateAsync(id, appointmentRequestDto, cancellationToken);

		return NoContent();
	}

	/// <summary>
	/// Delete appointment method
	/// </summary>
	/// <param name="id">Appointment's unique identier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[Produces("application/json")]
	[Authorize(Roles = "doctor,receptionist")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		await _appointmentService.DeleteAsync(id, cancellationToken);

		return NoContent();
	}

	/// <summary>
	/// Returns splitted available time periods for some date
	/// </summary>
	/// <param name="availableTimesRequestDto"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost("reservations")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAvailableAppointmentTimes(AvailableTimesRequestDto availableTimesRequestDto, CancellationToken cancellationToken)
	{
		var times = await _timeSlotService.GetAvailablePeriodsForDateAsync(availableTimesRequestDto, cancellationToken);

		return Ok(times);
	}
}