using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;

namespace Appointment.Application.Services.Interfaces;

public interface IResultService
{
	public Task<ResultResponseDto> GetByIdAsync(Guid id);
	public Task<IEnumerable<ResultResponseDto>> GetAllAsync();
	public Task<ResultForDownloadResponseDto> GetForDownloadAsync(Guid id);
	public Task<ResultResponseDto> CreateAsync(ResultRequestDto resultRequestDto);
	public Task UpdateAsync(Guid id, ResultRequestDto resultRequestDto);
	public Task DeleteAsync(Guid id);
}
