using MediatR;
using Offices.Core.Dto.Request;

namespace Offices.Application.CQRS.Offices.Commands.UpdateOffice;

public class UpdateOfficeCommand : IRequest
{
	public UpdateOfficeCommand(
		Guid id,
		OfficeRequestDto officeRequestDto)
	{
		Id = id;
		OfficeRequestDto = officeRequestDto;
	}

	public Guid Id { get; set; }
	public OfficeRequestDto OfficeRequestDto { get; set; }
}
