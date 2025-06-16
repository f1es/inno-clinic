using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Appointment.Infrastructure.Hubs;

[Authorize]
public class NotificationHub : Hub
{
	private ILogger<NotificationHub> _logger;

	public NotificationHub(ILogger<NotificationHub> logger)
	{
		_logger = logger;
	}

	public async Task SendNotification(string message)
	{
		var userId = Context.UserIdentifier;
		await Clients.User(userId).SendAsync(message);
	}

	public override Task OnConnectedAsync()
	{
		_logger.LogInformation($"Connected UserIdentifier = {Context.UserIdentifier}");
		return base.OnConnectedAsync();
	}
}
