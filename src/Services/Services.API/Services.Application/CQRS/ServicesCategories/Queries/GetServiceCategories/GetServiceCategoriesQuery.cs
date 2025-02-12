using MediatR;
using Services.Application.Dto.Response;

namespace Services.Application.CQRS.ServicesCategories.Queries.GetServiceCategories;

public record GetServiceCategoriesQuery() : IRequest<IEnumerable<ServiceCategoryResponseDto>>;
