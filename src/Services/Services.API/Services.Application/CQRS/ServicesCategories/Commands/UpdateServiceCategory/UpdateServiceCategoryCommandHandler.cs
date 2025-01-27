using MediatR;
using Services.Application.Mappers.Interfaces;
using Services.Core.Repositories;
using Shared.Exceptions;

namespace Services.Application.CQRS.ServicesCategories.Commands.UpdateServiceCategory;

public class UpdateServiceCategoryCommandHandler : IRequestHandler<UpdateServiceCategoryCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IServiceCategoriesMapper _serviceCategoriesMapper;

	public UpdateServiceCategoryCommandHandler(
		IUnitOfWork unitOfWork,
		IServiceCategoriesMapper serviceCategoriesMapper)
	{
		_unitOfWork = unitOfWork;
		_serviceCategoriesMapper = serviceCategoriesMapper;
	}

	public async Task Handle(UpdateServiceCategoryCommand request, CancellationToken cancellationToken)
	{
		var serviceCategory = await _unitOfWork.ServiceCategoryRepository.GetByIdAsync(request.Id, trackChanges: true);

		if (serviceCategory == null)
		{
			throw new NotFoundException(nameof(serviceCategory), request.Id);
		}

		serviceCategory.TimeSlotSize = request.ServiceCategoryRequestDto.TimeSlotSize;
		serviceCategory.CategoryName = request.ServiceCategoryRequestDto.CategoryName;

		await _unitOfWork.SaveAsync();
	}
}
