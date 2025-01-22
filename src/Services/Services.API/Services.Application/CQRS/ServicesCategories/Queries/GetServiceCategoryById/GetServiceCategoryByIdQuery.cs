using MediatR;
using Services.Core.Dto.Response;
using Services.Core.Models;

namespace Services.Application.CQRS.ServicesCategories.Queries.GetServiceCategoryById;

public record GetServiceCategoryByIdQuery(Guid Id) : IRequest<ServiceCategoryResponseDto>;
