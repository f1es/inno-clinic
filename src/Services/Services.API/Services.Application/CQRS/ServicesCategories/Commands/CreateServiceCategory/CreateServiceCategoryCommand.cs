using MediatR;
using Services.Core.Dto.Request;
using Services.Core.Dto.Response;

namespace Services.Application.CQRS.ServicesCategories.Commands.CreateServiceCategory;

public record CreateServiceCategoryCommand(ServiceCategoryRequestDto ServiceCategoryRequestDto) : IRequest<ServiceCategoryResponseDto>;
