using MediatR;
using Offices.Core.Dto.Request;

namespace Offices.Application.CQRS.Receptionists.Commands.UpdateReceptionist;

public class UpdateReceptionistCommand : IRequest
{
	public UpdateReceptionistCommand(Guid id, ReceptionistRequestDto receptionistRequestDto)
	{
		Id = id;
		ReceptionistRequestDto = receptionistRequestDto;
	}

	public Guid Id { get; set; }
	public ReceptionistRequestDto ReceptionistRequestDto { get; set; }
}
