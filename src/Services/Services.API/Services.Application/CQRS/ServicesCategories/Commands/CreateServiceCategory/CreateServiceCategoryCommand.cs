using MediatR;
using Services.Application.Dto.Request;
using Services.Application.Dto.Response;

namespace Services.Application.CQRS.ServicesCategories.Commands.CreateServiceCategory;

public record CreateServiceCategoryCommand(ServiceCategoryRequestDto ServiceCategoryRequestDto) : IRequest<ServiceCategoryResponseDto>;
