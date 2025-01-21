using AutoMapper;
using MediatR;
using Offices.Core.Models;
using Offices.Core.Repositories;
using Shared.Exceptions;

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
		var office = await _unitOfWork.OfficeRepository.GetByIdAsync(request.Id);

		if (office == null)
		{
			throw new NotFoundException(nameof(office), request.Id);
		}

		var newOffice = _mapper.Map<Office>(request.OfficeRequestDto);
		newOffice.Id = request.Id;

		await _unitOfWork.OfficeRepository.UpdateAsync(newOffice);
	}
}
