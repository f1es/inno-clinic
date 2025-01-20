using MediatR;
using Offices.Core.Dto.Request;
using Offices.Core.Dto.Response;

namespace Offices.Application.CQRS.Receptionists.Commands.CreateRecepcionist;

public class CreateReceptionistCommand : IRequest<ReceptionistResponseDto>
{
	public ReceptionistRequestDto ReceptionistRequestDto { get; set; }

	public CreateReceptionistCommand(ReceptionistRequestDto receptionistRequestDto)
	{
		ReceptionistRequestDto = receptionistRequestDto;
	}
}
