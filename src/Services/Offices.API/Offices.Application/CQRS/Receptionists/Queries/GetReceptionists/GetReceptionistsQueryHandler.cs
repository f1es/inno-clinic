using AutoMapper;
using MediatR;
using Offices.Core.Dto.Response;
using Offices.Core.Repositories;

namespace Offices.Application.CQRS.Receptionists.Queries.GetReceptionists;

public class GetReceptionistsQueryHandler : IRequestHandler<GetReceptionistsQuery, IEnumerable<ReceptionistResponseDto>>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public GetReceptionistsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<IEnumerable<ReceptionistResponseDto>> Handle(GetReceptionistsQuery request, CancellationToken cancellationToken)
	{
		var receptionists = await _unitOfWork.RecepcionistRepository.GetAllAsync();

		return _mapper.Map<IEnumerable<ReceptionistResponseDto>>(receptionists);
	}
}
