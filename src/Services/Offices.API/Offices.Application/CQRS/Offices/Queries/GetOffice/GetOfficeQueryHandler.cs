using AutoMapper;
using MediatR;
using Offices.Core.Cache;
using Offices.Core.Dto.Response;
using Offices.Core.Repositories;
using Shared.Exceptions;

namespace Offices.Application.CQRS.Offices.Queries.GetOffice;

public class GetOfficeQueryHandler : IRequestHandler<GetOfficeQuery, OfficeResponseDto>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IOfficesCacheService _officesCacheService;

	public GetOfficeQueryHandler(
		IMapper mapper,
		IUnitOfWork unitOfWork,
		IOfficesCacheService officesCacheService)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_officesCacheService = officesCacheService;
	}

	public async Task<OfficeResponseDto> Handle(GetOfficeQuery request, CancellationToken cancellationToken)
	{
		var office = await _officesCacheService.GetOfficeAsync(
			request.Id,
			() => _unitOfWork.OfficeRepository.GetByIdAsync(request.Id, cancellationToken),
			cancellationToken);

		if (office == null)
		{
			throw new NotFoundException(nameof(office), request.Id);
		}

		return _mapper.Map<OfficeResponseDto>(office);
	}
}
