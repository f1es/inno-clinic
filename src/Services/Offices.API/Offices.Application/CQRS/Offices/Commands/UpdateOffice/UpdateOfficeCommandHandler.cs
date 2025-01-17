using AutoMapper;
using MediatR;
using Offices.Core.Repositories;

namespace Offices.Application.CQRS.Offices.Commands.UpdateOffice;

public class UpdateOfficeCommandHandler : IRequestHandler<UpdateOfficeCommand>
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateOfficeCommandHandler(
		IMapper mapper,
		IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
	{
		var office = await _unitOfWork.OfficeRepository.GetByIdAsync(request.Id, trackChanges: true);

		if (office == null)
		{
			// 404 Exception
		}

		office = _mapper.Map(request.OfficeRequestDto, office);

		await _unitOfWork.SaveAsync();
	}
}
