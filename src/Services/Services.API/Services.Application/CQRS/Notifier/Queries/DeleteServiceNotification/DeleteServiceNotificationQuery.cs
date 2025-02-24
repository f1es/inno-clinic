using MediatR;

namespace Services.Application.CQRS.Notifier.Queries.DeleteServiceNotification;

public record DeleteServiceNotificationQuery(Guid Id) : IRequest;
