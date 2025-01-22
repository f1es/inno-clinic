using MediatR;
using Offices.Core.Dto.Request;

namespace Offices.Application.CQRS.Offices.Commands.UpdateOffice;

public record UpdateOfficeCommand(
	Guid Id, 
	OfficeRequestDto OfficeRequestDto) : IRequest
{ }
