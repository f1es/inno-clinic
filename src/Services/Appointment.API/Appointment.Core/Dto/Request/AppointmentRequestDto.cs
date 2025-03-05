namespace Appointment.Core.Dto.Request;

public record AppointmentRequestDto(
	Guid? PatientId,
	Guid? DoctorId,
	Guid? ServiceId,
	DateOnly Date,
	TimeOnly Time,
	bool IsApproved);
