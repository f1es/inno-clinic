using AutoMapper;
using MediatR;
using Offices.Core.Dto.Response;
using Offices.Core.Repositories;

namespace Offices.Application.CQRS.Receptionists.Queries.GetReceptionist;

public class GetReceptionistQueryHandler : IRequestHandler<GetReceptionistQuery, ReceptionistResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetReceptionistQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReceptionistResponseDto> Handle(GetReceptionistQuery request, CancellationToken cancellationToken)
    {
        var receptionist = await _unitOfWork.RecepcionistRepository.GetByIdAsync(request.Id);

        if (receptionist == null)
        {
            // 404 Exception
        }

        var receptionistResponse = _mapper.Map<ReceptionistResponseDto>(receptionist);

        return receptionistResponse;
    }
}
