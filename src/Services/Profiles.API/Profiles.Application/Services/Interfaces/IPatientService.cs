using Profiles.Core.Dtos.Request;
using Profiles.Core.Dtos.Response;

namespace Profiles.Application.Services.Interfaces;

public interface IPatientService
{
	public Task<PatientResponseDto> GetByIdAsync(Guid id);
	public Task<IEnumerable<PatientResponseDto>> GetAllAsync();
	public Task<PatientResponseDto> CreateAsync(PatientRequestDto patientRequestDto);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Guid id, PatientRequestDto patientRequestDto);
}
