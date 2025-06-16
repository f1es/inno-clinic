namespace Appointment.Core.Notifiers;

public interface IDoctorNotificationSender
{
	public Task NotifyAsync(Guid doctorId, string message, CancellationToken cancellationToken = default);
}
