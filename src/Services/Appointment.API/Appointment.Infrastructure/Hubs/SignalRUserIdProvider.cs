using Microsoft.AspNetCore.SignalR;

namespace Appointment.Infrastructure.Hubs;

public class SignalRUserIdProvider : IUserIdProvider
{
	public string? GetUserId(HubConnectionContext connection)
	{
		return connection.User?.FindFirst("id")?.Value;
	}
}
