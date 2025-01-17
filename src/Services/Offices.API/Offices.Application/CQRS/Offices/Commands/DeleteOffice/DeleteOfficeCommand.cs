using MediatR;

namespace Offices.Application.CQRS.Offices.Commands.DeleteOffice;

public class DeleteOfficeCommand : IRequest
{
	public Guid Id { get; set; }
	public DeleteOfficeCommand(Guid id)
	{
		Id = id;
	}
}
