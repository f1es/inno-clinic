using Appointment.Application.Mappers.Interfaces;
using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;
using Riok.Mapperly.Abstractions;

namespace Appointment.Application.Mappers.Implementations;

[Mapper]
public partial class AppointmentsMapper : IAppointmentsMapper
{
	[MapperIgnoreTarget(nameof(Core.Models.Appointment.Id))]
	[MapperIgnoreTarget(nameof(Core.Models.Appointment.Result))]
	public partial Core.Models.Appointment ToModel(AppointmentRequestDto appointmentRequestDto);
	[MapperIgnoreSource(nameof(Core.Models.Appointment.Result))]
	public partial AppointmentResponseDto ToResponse(Core.Models.Appointment appointment);
	[MapperIgnoreSource(nameof(Core.Models.Appointment.Result))]
	public partial IEnumerable<AppointmentResponseDto> ToResponse(IEnumerable<Core.Models.Appointment> appointments);
}
