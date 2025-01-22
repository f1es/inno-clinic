using MediatR;
using Offices.Core.Dto.Request;
using Offices.Core.Dto.Response;

namespace Offices.Application.CQRS.Offices.Commands.CreateOffice;

public record CreateOfficeCommand(OfficeRequestDto OfficeRequestDto) : IRequest<OfficeResponseDto>
{ }
