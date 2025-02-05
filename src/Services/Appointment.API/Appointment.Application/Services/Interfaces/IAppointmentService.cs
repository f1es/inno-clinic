using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;

namespace Appointment.Application.Services.Interfaces;

public interface IAppointmentService
{
	public Task<AppointmentResponseDto> GetByIdAsync(Guid id);
	public Task<IEnumerable<AppointmentResponseDto>> GetAllAsync();
	public Task<AppointmentResponseDto> CreateAsync(AppointmentRequestDto appointmentRequestDto);
	public Task UpdateAsync(Guid id, AppointmentRequestDto appointmentRequestDto);
	public Task DeleteAsync(Guid id);
}
