using MediatR;
using Services.Core.Dto.Request;

namespace Services.Application.CQRS.ServicesCategories.Commands.UpdateServiceCategory;

public record UpdateServiceCategoryCommand(
	Guid Id,
	ServiceCategoryRequestDto ServiceCategoryRequestDto) : IRequest;
