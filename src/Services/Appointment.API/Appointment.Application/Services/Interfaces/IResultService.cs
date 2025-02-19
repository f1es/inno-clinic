using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;

namespace Appointment.Application.Services.Interfaces;

public interface IResultService
{
	public Task<ResultResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
	public Task<IEnumerable<ResultResponseDto>> GetAllAsync(CancellationToken cancellationToken);
	public Task<ResultForDownloadResponseDto> GetForDownloadAsync(Guid id, CancellationToken cancellationToken);
	public Task<ResultResponseDto> CreateAsync(ResultRequestDto resultRequestDto, CancellationToken cancellationToken);
	public Task UpdateAsync(Guid id, ResultRequestDto resultRequestDto, CancellationToken cancellationToken);
	public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
