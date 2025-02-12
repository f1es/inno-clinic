using MediatR;
using Services.Application.Dto.Response;
using Services.Application.Mappers.Interfaces;
using Services.Core.Repositories;
using Shared.Exceptions;

namespace Services.Application.CQRS.ServicesCategories.Queries.GetServiceCategoryById;

public class GetServiceCategoryByIdQueryHandler : IRequestHandler<GetServiceCategoryByIdQuery, ServiceCategoryResponseDto>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IServiceCategoriesMapper _serviceCategoriesMapper;

	public GetServiceCategoryByIdQueryHandler(
		IUnitOfWork unitOfWork, 
		IServiceCategoriesMapper serviceCategoriesMapper)
	{
		_unitOfWork = unitOfWork;
		_serviceCategoriesMapper = serviceCategoriesMapper;
	}

	public async Task<ServiceCategoryResponseDto> Handle(GetServiceCategoryByIdQuery request, CancellationToken cancellationToken)
	{
		var serviceCategory = await _unitOfWork.ServiceCategoryRepository.GetByIdAsync(request.Id, cancellationToken);

		if (serviceCategory == null)
		{
			throw new NotFoundException(nameof(serviceCategory), request.Id);
		}

		return _serviceCategoriesMapper.ToResponse(serviceCategory);
	}
}
