namespace Appointment.Core.Models;

public class Appointment
{
	public Guid Id { get; set; }
	public Guid? PatientId { get; set; }
	public Guid? DoctorId { get; set; }
	public Guid? ServiceId { get; set; }
	public DateOnly Date {  get; set; }
	public TimeOnly BeginTime { get; set; }
	public TimeOnly EndTime { get; set; }
	public bool IsApproved { get; set; }

	public Result Result { get; set; }
}
