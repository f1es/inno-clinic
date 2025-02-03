using Profiles.Application.Utility;
using Profiles.Core.Dtos.Request;
using Profiles.Core.Dtos.Response;
using Profiles.Core.Parameters;

namespace Profiles.Application.Services.Interfaces;

public interface ISpecializationService
{
	public Task<SpecializationResponseDto> GetByIdAsync(Guid id);
	public Task<PagedList<SpecializationResponseDto>> GetAllAsync(RequestParameters requestParameters);
	public Task<SpecializationResponseDto> CreateAsync(SpecializationRequestDto specializationRequestDto);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Guid id, SpecializationRequestDto specializationRequestDto);
}
