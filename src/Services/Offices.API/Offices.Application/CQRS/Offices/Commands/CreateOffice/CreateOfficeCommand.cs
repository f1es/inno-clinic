using MediatR;
using Offices.Core.Dto.Request;
using Offices.Core.Dto.Response;

namespace Offices.Application.CQRS.Offices.Commands.CreateOffice;

public class CreateOfficeCommand : IRequest<OfficeResponseDto>
{
	public OfficeRequestDto OfficeRequestDto { get; set; }
	public CreateOfficeCommand(OfficeRequestDto officeRequestDto)
	{
		OfficeRequestDto = officeRequestDto;
	}
}
