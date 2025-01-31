using Profiles.Core.Dtos.Request;
using Profiles.Core.Dtos.Response;

namespace Profiles.Application.Services.Interfaces;

public interface IReceptionistService
{
	public Task<ReceptionistResponseDto> GetByIdAsync(Guid id);
	public Task<IEnumerable<ReceptionistResponseDto>> GetAllAsync();
	public Task<ReceptionistResponseDto> CreateAsync(ReceptionistRequestDto receptionistRequestDto);
	public Task DeleteAsync(Guid id);
	public Task UpdateAsync(Guid id, ReceptionistRequestDto receptionistRequestDto);
}
