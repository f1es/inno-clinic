using Appointment.Core.Dto.Response;

namespace Appointment.Core.RequestClients;

public interface IPatientsRequestClient
{
	public Task<PatientResponseDto> GetPatientAsync(Guid patientId, CancellationToken cancellationToken);
}
