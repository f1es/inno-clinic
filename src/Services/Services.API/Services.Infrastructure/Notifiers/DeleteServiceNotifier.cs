using MassTransit;
using Services.Core.Notifiers;
using Shared.Messages;

namespace Services.Infrastructure.Notifiers;

public class DeleteServiceNotifier : IDeleteServiceNotifier
{
	private readonly IBus _bus;
	private const string _queueName = "DeleteService";

	public DeleteServiceNotifier(IBus bus)
	{
		_bus = bus;
	}

	public async Task NotifyAsync(Guid serviceId, CancellationToken cancellationToken)
	{
		var message = new DeleteServiceMessage(serviceId);
		var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{_queueName}"));
		await endpoint.Send(message, cancellationToken);
	}
}
