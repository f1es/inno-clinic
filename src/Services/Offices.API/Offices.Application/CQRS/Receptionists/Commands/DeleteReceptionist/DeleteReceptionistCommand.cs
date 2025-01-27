using MediatR;

namespace Offices.Application.CQRS.Receptionists.Commands.DeleteREceptionist;

public record DeleteReceptionistCommand(Guid Id) : IRequest
{ }
