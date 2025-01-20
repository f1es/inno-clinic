using MediatR;

namespace Offices.Application.CQRS.Receptionists.Commands.DeleteREceptionist;

public class DeleteReceptionistCommand : IRequest
{
	public DeleteReceptionistCommand(Guid id)
	{
		Id = id;
	}

	public Guid Id { get; set; }
}
