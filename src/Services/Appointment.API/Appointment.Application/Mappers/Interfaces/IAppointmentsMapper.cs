using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;

namespace Appointment.Application.Mappers.Interfaces;

public interface IAppointmentsMapper
{
    Core.Models.Appointment ToModel(AppointmentRequestDto appointmentRequestDto);
    AppointmentResponseDto ToResponse(Core.Models.Appointment appointment);
    IEnumerable<AppointmentResponseDto> ToResponse(IEnumerable<Core.Models.Appointment> appointments);
	void Update(AppointmentRequestDto destination, Core.Models.Appointment source);

}