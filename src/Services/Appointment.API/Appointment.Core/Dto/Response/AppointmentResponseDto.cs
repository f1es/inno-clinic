namespace Appointment.Core.Dto.Response;

public record AppointmentResponseDto(
	Guid Id,
	Guid? PatientId,
	Guid? DoctorId,
	Guid? ServiceId,
	DateOnly Date,
	TimeOnly BeginTime,
	TimeOnly EndTime,
	bool IsApproved);
