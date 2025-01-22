using MediatR;
using Services.Core.Dto.Request;
using Services.Core.Dto.Response;

namespace Services.Application.CQRS.Services.Commands.CreateService;

public record CreateServiceCommand(ServiceRequestDto ServiceRequestDto) : IRequest<ServiceResponseDto>;
