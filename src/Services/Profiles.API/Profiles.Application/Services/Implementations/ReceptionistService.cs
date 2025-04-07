using Mapster;
using Profiles.Application.Extensions;
using Profiles.Application.Services.Interfaces;
using Profiles.Core.Dtos.Request;
using Profiles.Core.Dtos.Response;
using Profiles.Core.Models;
using Profiles.Core.Parameters;
using Profiles.Core.Publishers;
using Profiles.Core.Repositories;
using Profiles.Core.Utility;
using Shared.Exceptions;
using Shared.Queues.Messages;

namespace Profiles.Application.Services.Implementations;

public class ReceptionistService : IReceptionistService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IFullNamePublisher _fullNamePublisher;

	public ReceptionistService(IUnitOfWork unitOfWork, IFullNamePublisher fullNamePublisher)
	{
		_unitOfWork = unitOfWork;
		_fullNamePublisher = fullNamePublisher;
	}

	public async Task<ReceptionistResponseDto> CreateAsync(ReceptionistRequestDto receptionistRequestDto)
	{
		var receptionist = receptionistRequestDto.Adapt<Receptionist>();

		_unitOfWork.ReceptionistRepository.Create(receptionist);

		await _unitOfWork.SaveAsync();

		var updateFullNameMessage = new UpdateFullNameMessage(
			receptionist.AccountId,
			receptionist.FirstName,
			receptionist.LastName,
			receptionist.MiddleName);
		await _fullNamePublisher.PublishUpdateAsync(updateFullNameMessage, cancellationToken: default);

		return receptionist.Adapt<ReceptionistResponseDto>();
	}

	public async Task DeleteAsync(Guid id)
	{
		var receptionist = await _unitOfWork.ReceptionistRepository.GetByIdAsync(id);

		ReceptionistNullCheck(receptionist, id);

		_unitOfWork.ReceptionistRepository.Delete(receptionist);

		await _unitOfWork.SaveAsync();

		var deleteFullNameMessage = new DeleteFullNameMessage(receptionist.AccountId);
		await _fullNamePublisher.PublishDeleteAsync(deleteFullNameMessage, cancellationToken: default);
	}

	public async Task<PagedList<ReceptionistResponseDto>> GetAllAsync(RequestParameters requestParameters)
	{
		var receptionists = await _unitOfWork.ReceptionistRepository.GetAllAsync(requestParameters);

		return receptionists.AdaptPagedList<Receptionist, ReceptionistResponseDto>();
	}

	public async Task<ReceptionistResponseDto> GetByIdAsync(Guid id)
	{
		var receptionist = await _unitOfWork.ReceptionistRepository.GetByIdAsync(id);

		ReceptionistNullCheck(receptionist, id);

		return receptionist.Adapt<ReceptionistResponseDto>();
	}

	public async Task UpdateAsync(Guid id, ReceptionistRequestDto receptionistRequestDto)
	{
		var receptionist = await _unitOfWork.ReceptionistRepository.GetByIdAsync(id, trackChanges: true);

		ReceptionistNullCheck(receptionist, id);

		receptionistRequestDto.Adapt(receptionist);

		await _unitOfWork.SaveAsync();

		var updateFullNameMessage = new UpdateFullNameMessage(
			receptionist.AccountId,
			receptionist.FirstName,
			receptionist.LastName,
			receptionist.MiddleName);
		await _fullNamePublisher.PublishUpdateAsync(updateFullNameMessage, cancellationToken: default);
	}

	private Receptionist ReceptionistNullCheck(Receptionist receptionist, Guid id) => receptionist ?? throw new NotFoundException(nameof(receptionist), id); 
}
