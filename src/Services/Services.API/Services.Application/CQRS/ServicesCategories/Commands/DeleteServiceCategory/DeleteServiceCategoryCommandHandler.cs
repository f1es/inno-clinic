using MediatR;
using Services.Application.Mappers.Interfaces;
using Services.Core.Repositories;

namespace Services.Application.CQRS.ServicesCategories.Commands.DeleteServiceCategory;

public class DeleteServiceCategoryCommandHandler : IRequestHandler<DeleteServiceCategoryCommand>
{
	private readonly IUnitOfWork _unitOfWork;

	public DeleteServiceCategoryCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task Handle(DeleteServiceCategoryCommand request, CancellationToken cancellationToken)
	{
		var serviceCategory = await _unitOfWork.ServiceCategoryRepository.GetByIdAsync(request.Id);

		if (serviceCategory == null)
		{
			// 404 ex
		}

		_unitOfWork.ServiceCategoryRepository.Delete(serviceCategory);

		await _unitOfWork.SaveAsync();
	}
}
