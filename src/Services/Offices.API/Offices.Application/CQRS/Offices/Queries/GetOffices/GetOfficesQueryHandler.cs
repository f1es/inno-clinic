using AutoMapper;
using MediatR;
using Offices.Core.Dto.Response;
using Offices.Core.Repositories;

namespace Offices.Application.CQRS.Offices.Queries.GetOffices;

public class GetOfficesQueryHandler : IRequestHandler<GetOfficesQuery, IEnumerable<OfficeResponseDto>>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public GetOfficesQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<IEnumerable<OfficeResponseDto>> Handle(GetOfficesQuery request, CancellationToken cancellationToken)
	{
		var offices = await _unitOfWork.OfficeRepository.GetAllAsync();

		var officesResponse = _mapper.Map<IEnumerable<OfficeResponseDto>>(offices);

		return officesResponse;
	}
}
