using AutoMapper;
using MediatR;
using Offices.Core.Dto.Response;
using Offices.Core.Repositories;
using Shared.Exceptions;

namespace Offices.Application.CQRS.Offices.Queries.GetOffice;

public class GetOfficeQueryHandler : IRequestHandler<GetOfficeQuery, OfficeResponseDto>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public GetOfficeQueryHandler(
		IMapper mapper,
		IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<OfficeResponseDto> Handle(GetOfficeQuery request, CancellationToken cancellationToken)
	{
		var office = await _unitOfWork.OfficeRepository.GetByIdAsync(request.Id);

		if (office == null)
		{
			throw new NotFoundException(nameof(office), request.Id);
		}

		return _mapper.Map<OfficeResponseDto>(office);
	}
}
