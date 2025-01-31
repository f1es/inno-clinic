using Profiles.Core.Dtos.Request;
using Profiles.Core.Dtos.Response;

namespace Profiles.Application.Services.Interfaces;

public interface ISpecializationService
{
	public Task<SpecializationResponseDto> GetByIdAsync(Guid id);
	public Task<IEnumerable<SpecializationResponseDto>> GetAllAsync();
	public Task<SpecializationResponseDto> CreateAsync(SpecializationRequestDto specializationRequestDto);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Guid id, SpecializationRequestDto specializationRequestDto);
}
