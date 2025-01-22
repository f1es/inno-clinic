using MediatR;
using Offices.Core.Dto.Request;
using Offices.Core.Dto.Response;

namespace Offices.Application.CQRS.Receptionists.Commands.CreateRecepcionist;

public record CreateReceptionistCommand(ReceptionistRequestDto ReceptionistRequestDto) : IRequest<ReceptionistResponseDto>
{ }
