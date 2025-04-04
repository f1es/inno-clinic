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

public class PatientService : IPatientService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IFullNamePublisher _fullNamePublisher;

	public PatientService(IUnitOfWork unitOfWork, IFullNamePublisher fullNamePublisher)
	{
		_unitOfWork = unitOfWork;
		_fullNamePublisher = fullNamePublisher;
	}

	public async Task<PatientResponseDto> CreateAsync(PatientRequestDto patientRequestDto)
	{
		var patient = patientRequestDto.Adapt<Patient>();

		_unitOfWork.PatientRepository.Create(patient);

		await _unitOfWork.SaveAsync();

		if (patient.AccountId is not null)
		{
			var updateFullNameMessage = new UpdateFullNameMessage(
				patient.AccountId.Value,
				patient.FirstName,
				patient.LastName,
				patient.MiddleName);
			await _fullNamePublisher.PublishUpdateAsync(updateFullNameMessage, cancellationToken: default);
		}

		return patient.Adapt<PatientResponseDto>();
	}

	public async Task DeleteAsync(Guid id)
	{
		var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id);

		PatientNullCheck(patient, id);

		_unitOfWork.PatientRepository.Delete(patient);

		await _unitOfWork.SaveAsync();

		if (patient.AccountId is not null)
		{
			var deleteFullNameMessage = new DeleteFullNameMessage(
				patient.AccountId.Value);
			await _fullNamePublisher.PublishDeleteAsync(deleteFullNameMessage, cancellationToken: default);
		}
	}

	public async Task<PagedList<PatientResponseDto>> GetAllAsync(RequestParameters requestParameters)
	{
		var patients = await _unitOfWork.PatientRepository.GetAllAsync(requestParameters);

		return patients.AdaptPagedList<Patient, PatientResponseDto>();
	}

	public async Task<PatientResponseDto> GetByIdAsync(Guid id)
	{
		var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id);

		PatientNullCheck(patient, id);

		return patient.Adapt<PatientResponseDto>();
	}

	public async Task UpdateAsync(Guid id, PatientRequestDto patientRequestDto)
	{
		var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id, trackChanges: true);

		PatientNullCheck(patient, id);

		patientRequestDto.Adapt(patient);

		await _unitOfWork.SaveAsync();

		if (patient.AccountId is not null)
		{
			var updateFullNameMessage = new UpdateFullNameMessage(
				patient.AccountId.Value,
				patient.FirstName,
				patient.LastName,
				patient.MiddleName);
			await _fullNamePublisher.PublishUpdateAsync(updateFullNameMessage, cancellationToken: default);
		}
	}

	private Patient PatientNullCheck(Patient patient, Guid id) => patient ?? throw new NotFoundException(nameof(patient), id); 
}
