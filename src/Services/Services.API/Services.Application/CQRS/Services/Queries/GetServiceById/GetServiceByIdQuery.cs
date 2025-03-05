using MediatR;
using Services.Application.Dto.Response;

namespace Services.Application.CQRS.Services.Queries.GetServiceById;

public record GetServiceByIdQuery(Guid Id) : IRequest<ServiceWithCategoryResponseDto>;
