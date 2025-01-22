using MediatR;

namespace Offices.Application.CQRS.Offices.Commands.DeleteOffice;

public record DeleteOfficeCommand(Guid Id) : IRequest
{ }
