namespace Appointment.Application.Services.Interfaces;

public interface INotifyService
{
	public Task NotifyAsync(Guid patientId, Guid appointmentId, CancellationToken cancellationToken);
}
