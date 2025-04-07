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

public class DoctorService : IDoctorService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IFullNamePublisher _fullNamePublisher;

	public DoctorService(IUnitOfWork unitOfWork, IFullNamePublisher fullNamePublisher)
	{
		_unitOfWork = unitOfWork;
		_fullNamePublisher = fullNamePublisher;
	}

	public async Task<DoctorResponseDto> CreateAsync(DoctorRequestDto doctorRequestDto)
	{
		var doctor = doctorRequestDto.Adapt<Doctor>();

		_unitOfWork.DoctorRepository.Create(doctor);

		await _unitOfWork.SaveAsync();

		var updateFullNameMessage = new UpdateFullNameMessage(
			doctor.AccountId,
			doctor.FirstName,
			doctor.LastName,
			doctor.MiddleName);
		await _fullNamePublisher.PublishUpdateAsync(updateFullNameMessage, cancellationToken: default);

		return doctor.Adapt<DoctorResponseDto>();
	}

	public async Task DeleteAsync(Guid id)
	{
		var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id);

		DoctorNullCheck(doctor, id);

		_unitOfWork.DoctorRepository.Delete(doctor);

		await _unitOfWork.SaveAsync();

		var deleteFullNameMessage = new DeleteFullNameMessage(doctor.AccountId);
		await _fullNamePublisher.PublishDeleteAsync(deleteFullNameMessage, cancellationToken: default);
	}

	public async Task<PagedList<DoctorResponseDto>> GetAllAsync(RequestParameters requestParameters)
	{
		var doctors = await _unitOfWork.DoctorRepository.GetAllAsync(requestParameters);

		return doctors.AdaptPagedList<Doctor, DoctorResponseDto>();
	}

	public async Task<DoctorResponseDto> GetByIdAsync(Guid id)
	{
		var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id);

		DoctorNullCheck(doctor, id);

		return doctor.Adapt<DoctorResponseDto>();
	}

	public async Task UpdateAsync(Guid id, DoctorRequestDto doctorRequestDto)
	{
		var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id, trackChanges: true);

		DoctorNullCheck(doctor, id);

		doctorRequestDto.Adapt(doctor);

		await _unitOfWork.SaveAsync();

		var updateFullNameMessage = new UpdateFullNameMessage(
			doctor.AccountId,
			doctor.FirstName,
			doctor.LastName,
			doctor.MiddleName);
		await _fullNamePublisher.PublishUpdateAsync(updateFullNameMessage, cancellationToken: default);
	}

	private Doctor DoctorNullCheck(Doctor doctor, Guid id) => doctor ?? throw new NotFoundException(nameof(doctor), id);
}
