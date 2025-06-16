using Appointment.Core.Notifiers;
using Appointment.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Appointment.Infrastructure.Notifiers;

public class DoctorNotificationSender : IDoctorNotificationSender
{
	private readonly IHubContext<NotificationHub> _hub;

	public DoctorNotificationSender(IHubContext<NotificationHub> hub)
	{
		_hub = hub;
	}

	public async Task NotifyAsync(Guid accountId, string message, CancellationToken cancellationToken = default)
	{
		await _hub.Clients.User(accountId.ToString()).SendAsync("ReceiveMessage", message, cancellationToken);
	}
}
