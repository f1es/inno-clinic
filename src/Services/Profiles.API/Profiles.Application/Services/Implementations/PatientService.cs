using Mapster;
using Profiles.Application.Services.Interfaces;
using Profiles.Application.Extensions;
using Profiles.Core.Dtos.Request;
using Profiles.Core.Dtos.Response;
using Profiles.Core.Models;
using Profiles.Core.Parameters;
using Profiles.Core.Repositories;
using Profiles.Core.Utility;
using Shared.Exceptions;

namespace Profiles.Application.Services.Implementations;

public class PatientService : IPatientService
{
	private readonly IUnitOfWork _unitOfWork;

	public PatientService(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<PatientResponseDto> CreateAsync(PatientRequestDto patientRequestDto)
	{
		var patient = patientRequestDto.Adapt<Patient>();

		_unitOfWork.PatientRepository.Create(patient);

		await _unitOfWork.SaveAsync();

		return patient.Adapt<PatientResponseDto>();
	}

	public async Task DeleteAsync(Guid id)
	{
		var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id);

		PatientNullCheck(patient, id);

		_unitOfWork.PatientRepository.Delete(patient);

		await _unitOfWork.SaveAsync();
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
	}

	private Patient PatientNullCheck(Patient patient, Guid id) => patient ?? throw new NotFoundException(nameof(patient), id); 
}
