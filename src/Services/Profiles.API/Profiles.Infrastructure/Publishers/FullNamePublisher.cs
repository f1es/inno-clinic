using MassTransit;
using Profiles.Core.Publishers;
using Shared.Queues;
using Shared.Queues.Messages;

namespace Profiles.Infrastructure.Publishers;

public class FullNamePublisher : IFullNamePublisher
{
	private readonly IBus _bus;

	public FullNamePublisher(IBus bus)
	{
		_bus = bus;
	}

	public async Task PublishUpdateAsync(UpdateFullNameMessage updateFullNameMessage, CancellationToken cancellationToken)
	{
		var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{QueueNames.FullNameQueue}"));
		await endpoint.Send(updateFullNameMessage, cancellationToken);
	}

	public async Task PublishDeleteAsync(DeleteFullNameMessage deleteFullNameMessage, CancellationToken cancellationToken)
	{
		var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{QueueNames.FullNameQueue}"));
		await endpoint.Send(deleteFullNameMessage, cancellationToken);
	}
}
