using MediatR;
using Services.Application.Mappers.Interfaces;
using Services.Core.Dto.Response;
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

		_unitOfWork.ServiceCategoryRepository.Create(serviceCategory);

		await _unitOfWork.SaveAsync();

		return _serviceCategoriesMapper.ToResponse(serviceCategory);
	}
}
