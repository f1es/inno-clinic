using Mapster;
using Profiles.Application.Services.Interfaces;
using Profiles.Core.Dtos.Request;
using Profiles.Core.Dtos.Response;
using Profiles.Core.Models;
using Profiles.Core.Repositories;
using Shared.Exceptions;

namespace Profiles.Application.Services.Implementations;

public class DoctorService : IDoctorService
{
	private readonly IUnitOfWork _unitOfWork;

	public DoctorService(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<DoctorResponseDto> CreateAsync(DoctorRequestDto doctorRequestDto)
	{
		var doctor = doctorRequestDto.Adapt<Doctor>();

		_unitOfWork.DoctorRepository.Create(doctor);

		await _unitOfWork.SaveAsync();

		return doctor.Adapt<DoctorResponseDto>();
	}

	public async Task DeleteAsync(Guid id)
	{
		var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id);

		DoctorNullCheck(doctor, id);

		_unitOfWork.DoctorRepository.Delete(doctor);

		await _unitOfWork.SaveAsync();
	}

	public async Task<IEnumerable<DoctorResponseDto>> GetAllAsync()
	{
		var doctors = await _unitOfWork.DoctorRepository.GetAllAsync();

		return doctors.Adapt<IEnumerable<DoctorResponseDto>>();
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
	}

	private Doctor DoctorNullCheck(Doctor doctor, Guid id) => doctor ?? throw new NotFoundException(nameof(doctor), id);
}
