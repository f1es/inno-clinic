using MediatR;
using Services.Application.Mappers.Interfaces;
using Services.Core.Dto.Response;
using Services.Core.Repositories;

namespace Services.Application.CQRS.ServicesCategories.Queries.GetServiceCategories;

public class GetServiceCategoriesQueryHandler : IRequestHandler<GetServiceCategoriesQuery, IEnumerable<ServiceCategoryResponseDto>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IServiceCategoriesMapper _serviceCategoriesMapper;

	public GetServiceCategoriesQueryHandler(
		IUnitOfWork unitOfWork,
		IServiceCategoriesMapper serviceCategoriesMapper)
	{
		_unitOfWork = unitOfWork;
		_serviceCategoriesMapper = serviceCategoriesMapper;
	}

	public async Task<IEnumerable<ServiceCategoryResponseDto>> Handle(GetServiceCategoriesQuery request, CancellationToken cancellationToken)
	{
		var serviceCategories = await _unitOfWork.ServiceCategoryRepository.GetAllAsync(cancellationToken);

		return _serviceCategoriesMapper.ToResponse(serviceCategories);
	}
}
