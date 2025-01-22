using MediatR;
using Offices.Core.Dto.Request;

namespace Offices.Application.CQRS.Receptionists.Commands.UpdateReceptionist;

public record UpdateReceptionistCommand(
	Guid Id,
	ReceptionistRequestDto ReceptionistRequestDto) : IRequest
{ }
