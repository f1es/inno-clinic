using MediatR;
using Services.Core.Dto.Response;

namespace Services.Application.CQRS.Services.Queries.GetServices;

public record GetServicesQuery() : IRequest<IEnumerable<ServiceResponseDto>>;
