using MediatR;
using Services.Application.Dto.Response;
using Services.Application.Mappers.Interfaces;
using Services.Core.Repositories;

namespace Services.Application.CQRS.ServicesCategories.Commands.CreateServiceCategory;

public class CreateServiceCategoryCommandHandler : IRequestHandler<CreateServiceCategoryCommand, ServiceCategoryResponseDto>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IServiceCategoriesMapper _serviceCategoriesMapper;

	public CreateServiceCategoryCommandHandler(
		IUnitOfWork unitOfWork,
		IServiceCategoriesMapper serviceCategoriesMapper)
	{
		_unitOfWork = unitOfWork;
		_serviceCategoriesMapper = serviceCategoriesMapper;
	}

	public async Task<ServiceCategoryResponseDto> Handle(CreateServiceCategoryCommand request, CancellationToken cancellationToken)
	{
		var serviceCategory = _serviceCategoriesMapper.ToModel(request.ServiceCategoryRequestDto);

		serviceCategory.Id = Guid.NewGuid();
		_unitOfWork.ServiceCategoryRepository.Create(serviceCategory);

		await _unitOfWork.SaveAsync(cancellationToken);

		return _serviceCategoriesMapper.ToResponse(serviceCategory);
	}
}
