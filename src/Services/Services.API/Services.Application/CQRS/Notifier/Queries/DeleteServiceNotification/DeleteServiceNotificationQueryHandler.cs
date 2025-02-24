using MassTransit;
using MediatR;

namespace Services.Application.CQRS.Notifier.Queries.DeleteServiceNotification;

public class DeleteServiceNotificationQueryHandler : IRequestHandler<DeleteServiceNotificationQuery>
{
    private readonly IBus _bus;
    
    public DeleteServiceNotificationQueryHandler(IBus bus)
    {
        _bus = bus;
    }

    public async Task Handle(DeleteServiceNotificationQuery request, CancellationToken cancellationToken)
    {
        var endpoint = await _bus.GetSendEndpoint(new Uri("queue:Services.Application.CQRS.Notifier.Queries.DeleteServiceNotification"));
        await endpoint.Send(request, cancellationToken);
    }
}
