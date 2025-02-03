using Profiles.Application.Utility;
using Profiles.Core.Dtos.Request;
using Profiles.Core.Dtos.Response;
using Profiles.Core.Parameters;

namespace Profiles.Application.Services.Interfaces;

public interface IDoctorService
{
	public Task<DoctorResponseDto> GetByIdAsync(Guid id);
	public Task<PagedList<DoctorResponseDto>> GetAllAsync(RequestParameters requestParameters);
	public Task<DoctorResponseDto> CreateAsync(DoctorRequestDto doctorRequestDto);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Guid id, DoctorRequestDto doctorRequestDto);
}
