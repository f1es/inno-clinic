using MediatR;
using Services.Core.Dto.Response;

namespace Services.Application.CQRS.Services.Queries.GetServiceById;

public record GetServiceByIdQuery(Guid Id) : IRequest<ServiceResponseDto>;
