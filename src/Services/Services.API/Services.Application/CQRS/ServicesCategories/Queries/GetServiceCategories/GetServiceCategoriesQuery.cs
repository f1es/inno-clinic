using MediatR;
using Services.Core.Dto.Response;
using Services.Core.Models;

namespace Services.Application.CQRS.ServicesCategories.Queries.GetServiceCategories;

public record GetServiceCategoriesQuery() : IRequest<IEnumerable<ServiceCategoryResponseDto>>;
