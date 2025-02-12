using MediatR;
using Services.Application.Dto.Request;
using Services.Application.Dto.Response;

namespace Services.Application.CQRS.Services.Commands.CreateService;

public record CreateServiceCommand(ServiceRequestDto ServiceRequestDto) : IRequest<ServiceResponseDto>;
