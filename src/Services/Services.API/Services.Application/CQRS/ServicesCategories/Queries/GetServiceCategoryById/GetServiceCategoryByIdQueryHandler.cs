using MediatR;
using Services.Application.Mappers.Interfaces;
using Services.Core.Dto.Response;
using Services.Core.Repositories;

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
		var serviceCategory = await _unitOfWork.ServiceCategoryRepository.GetByIdAsync(request.Id);

		if (serviceCategory == null)
		{
			// ex 404
		}

		return _serviceCategoriesMapper.ToResponse(serviceCategory);
	}
}
