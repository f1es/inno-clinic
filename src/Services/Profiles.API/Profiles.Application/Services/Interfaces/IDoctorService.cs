using Profiles.Core.Dtos.Request;
using Profiles.Core.Dtos.Response;

namespace Profiles.Application.Services.Interfaces;

public interface IDoctorService
{
	public Task<DoctorResponseDto> GetByIdAsync(Guid id);
	public Task<IEnumerable<DoctorResponseDto>> GetAllAsync();
	public Task<DoctorResponseDto> CreateAsync(DoctorRequestDto doctorRequestDto);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Guid id, DoctorRequestDto doctorRequestDto);
}
