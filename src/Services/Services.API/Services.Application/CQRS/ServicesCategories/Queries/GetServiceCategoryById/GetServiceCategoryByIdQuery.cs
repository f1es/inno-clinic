using MediatR;
using Services.Application.Dto.Response;

namespace Services.Application.CQRS.ServicesCategories.Queries.GetServiceCategoryById;

public record GetServiceCategoryByIdQuery(Guid Id) : IRequest<ServiceCategoryResponseDto>;
