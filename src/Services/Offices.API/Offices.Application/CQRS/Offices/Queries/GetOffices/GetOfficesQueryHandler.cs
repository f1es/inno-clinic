using AutoMapper;
using MediatR;
using Offices.Core.Cache;
using Offices.Core.Dto.Response;
using Offices.Core.Repositories;

namespace Offices.Application.CQRS.Offices.Queries.GetOffices;

public class GetOfficesQueryHandler : IRequestHandler<GetOfficesQuery, IEnumerable<OfficeResponseDto>>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IOfficesCacheService _officesCacheService;

	public GetOfficesQueryHandler(
		IMapper mapper, 
		IUnitOfWork unitOfWork,
		IOfficesCacheService officesCacheService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_officesCacheService = officesCacheService;
	}

	public async Task<IEnumerable<OfficeResponseDto>> Handle(GetOfficesQuery request, CancellationToken cancellationToken)
	{
		var offices = await _officesCacheService.GetAllOfficesAsync(
			() => _unitOfWork.OfficeRepository.GetAllAsync(cancellationToken),
			cancellationToken);

		return _mapper.Map<IEnumerable<OfficeResponseDto>>(offices);
	}
}
