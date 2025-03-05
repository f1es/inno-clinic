using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;

namespace Appointment.Application.Services.Interfaces;

public interface IAppointmentService
{
	public Task<AppointmentResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
	public Task<IEnumerable<AppointmentResponseDto>> GetAllAsync(CancellationToken cancellationToken);
	public Task<AppointmentResponseDto> CreateAsync(AppointmentRequestDto appointmentRequestDto, CancellationToken cancellationToken);
	public Task UpdateAsync(Guid id, AppointmentRequestDto appointmentRequestDto, CancellationToken cancellationToken);
	public Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
