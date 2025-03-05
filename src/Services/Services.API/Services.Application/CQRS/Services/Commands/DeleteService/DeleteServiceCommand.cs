using MediatR;

namespace Services.Application.CQRS.Services.Commands.DeleteService;

public record DeleteServiceCommand(Guid Id) : IRequest;

