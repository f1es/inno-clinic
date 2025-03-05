using Profiles.Core.Dtos.Request;
using Profiles.Core.Dtos.Response;
using Profiles.Core.Parameters;
using Profiles.Core.Utility;

namespace Profiles.Application.Services.Interfaces;

public interface IReceptionistService
{
	public Task<ReceptionistResponseDto> GetByIdAsync(Guid id);
	public Task<PagedList<ReceptionistResponseDto>> GetAllAsync(RequestParameters requestParameters);
	public Task<ReceptionistResponseDto> CreateAsync(ReceptionistRequestDto receptionistRequestDto);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Guid id, ReceptionistRequestDto receptionistRequestDto);
}
