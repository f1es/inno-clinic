namespace Appointment.Core.Dto.Request;

public record AppointmentRequestDto(
	Guid? PatientId,
	Guid? DoctorId,
	Guid? ServiceId,
	Guid? AccountId,
	DateOnly Date,
	TimeOnly BeginTime,
	TimeOnly EndTime,
	bool IsApproved);
