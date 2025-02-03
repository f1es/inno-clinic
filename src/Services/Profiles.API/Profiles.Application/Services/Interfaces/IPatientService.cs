using Profiles.Application.Utility;
using Profiles.Core.Dtos.Request;
using Profiles.Core.Dtos.Response;
using Profiles.Core.Parameters;

namespace Profiles.Application.Services.Interfaces;

public interface IPatientService
{
	public Task<PatientResponseDto> GetByIdAsync(Guid id);
	public Task<PagedList<PatientResponseDto>> GetAllAsync(RequestParameters requestParameters);
	public Task<PatientResponseDto> CreateAsync(PatientRequestDto patientRequestDto);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Guid id, PatientRequestDto patientRequestDto);
}
