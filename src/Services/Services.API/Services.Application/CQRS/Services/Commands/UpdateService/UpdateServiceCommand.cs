using MediatR;
using Services.Core.Dto.Request;

namespace Services.Application.CQRS.Services.Commands.UpdateService;

public record UpdateServiceCommand(
	Guid Id, 
	ServiceRequestDto ServiceRequestDto) : IRequest;
