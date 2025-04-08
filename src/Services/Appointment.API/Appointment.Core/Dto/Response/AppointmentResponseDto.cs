using Appointment.Core.Models;

namespace Appointment.Core.Dto.Response;

public record AppointmentResponseDto(
	Guid Id,
	Guid? PatientId,
	Guid? DoctorId,
	Guid? ServiceId,
	Guid? AccountId,
	DateOnly Date,
	TimeOnly BeginTime,
	TimeOnly EndTime,
	bool IsApproved,
	ResultResponseDto? Result);