using MediatR;
using Services.Application.Dto.Response;

namespace Services.Application.CQRS.Services.Queries.GetServices;

public record GetServicesQuery() : IRequest<IEnumerable<ServiceResponseDto>>;
