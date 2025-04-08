using Appointment.Application.Mappers.Interfaces;
using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;
using Riok.Mapperly.Abstractions;

namespace Appointment.Application.Mappers.Implementations;

[Mapper(AllowNullPropertyAssignment = true)]
public partial class AppointmentsMapper : IAppointmentsMapper
{
	private readonly IResultsMapper _resultsMapper;

	public AppointmentsMapper(IResultsMapper resultsMapper)
	{
		_resultsMapper = resultsMapper;
	}

	[MapperIgnoreTarget(nameof(Core.Models.Appointment.Id))]
	[MapperIgnoreTarget(nameof(Core.Models.Appointment.Result))]
	public partial Core.Models.Appointment ToModel(AppointmentRequestDto appointmentRequestDto);
	public AppointmentResponseDto ToResponse(Core.Models.Appointment appointment)
	{
		var result = _resultsMapper.ToResponse(appointment.Result);

		return new AppointmentResponseDto(
			appointment.Id,
			appointment.PatientId,
			appointment.DoctorId,
			appointment.ServiceId,
			appointment.AccountId,
			appointment.Date,
			appointment.BeginTime,
			appointment.EndTime,
			appointment.IsApproved,
			result);
	}
	[MapperIgnoreSource(nameof(Core.Models.Appointment.Result))]
	public partial IEnumerable<AppointmentResponseDto> ToResponse(IEnumerable<Core.Models.Appointment> appointments);
	[MapperIgnoreTarget(nameof(Core.Models.Appointment.Id))]
	[MapperIgnoreTarget(nameof(Core.Models.Appointment.Result))]
	public partial void Update(AppointmentRequestDto source, Core.Models.Appointment destination);
}
