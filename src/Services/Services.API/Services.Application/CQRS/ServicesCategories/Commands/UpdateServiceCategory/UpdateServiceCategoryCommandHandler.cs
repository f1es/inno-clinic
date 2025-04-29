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
		var serviceCategory = await _unitOfWork.ServiceCategoryRepository.GetByIdAsync(request.Id, cancellationToken, trackChanges: true);

		if (serviceCategory == null)
		{
			throw new NotFoundException(nameof(serviceCategory), request.Id);
		}

		_serviceCategoriesMapper.UpdateModel(request.ServiceCategoryRequestDto, serviceCategory);
		_unitOfWork.ServiceCategoryRepository.Update(serviceCategory);

		await _unitOfWork.SaveAsync(cancellationToken);
	}
}
