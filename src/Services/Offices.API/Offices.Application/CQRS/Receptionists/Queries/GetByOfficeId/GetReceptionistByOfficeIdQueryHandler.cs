using AutoMapper;
using MediatR;
using Offices.Core.Dto.Response;
using Offices.Core.Repositories;
using Shared.Exceptions;

namespace Offices.Application.CQRS.Receptionists.Queries.GetByOfficeId;

public class GetReceptionistByOfficeIdQueryHandler : IRequestHandler<GetReceptionistByOfficeIdQuery, ReceptionistResponseDto>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public GetReceptionistByOfficeIdQueryHandler(
		IMapper mapper,
		IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<ReceptionistResponseDto> Handle(GetReceptionistByOfficeIdQuery request, CancellationToken cancellationToken)
	{
		var receptionist = await _unitOfWork.RecepcionistRepository.GetByOfficeIdAsync(request.OfficeId);

		if (receptionist == null)
		{
			throw new NotFoundException(nameof(receptionist), request.OfficeId);
		}

		var receptionistResponse = _mapper.Map<ReceptionistResponseDto>(receptionist);

		return receptionistResponse;
	}
}
