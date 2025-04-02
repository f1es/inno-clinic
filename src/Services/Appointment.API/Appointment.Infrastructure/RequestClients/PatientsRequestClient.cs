using Appointment.Core.Dto.Response;
using Appointment.Core.RequestClients;
using Appointment.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Appointment.Infrastructure.RequestClients;

public class PatientsRequestClient : IPatientsRequestClient
{
	private readonly PatientsEndpoints _patientsEndpoints;
	private readonly HttpClient _httpClient;

	public PatientsRequestClient(IOptions<PatientsEndpoints> servicesEndpoints, HttpClient httpClient)
	{
		_patientsEndpoints = servicesEndpoints.Value;
		_httpClient = httpClient;
	}

	public async Task<PatientResponseDto> GetPatientAsync(Guid patientId, CancellationToken cancellationToken)
	{
		var uri = $"{_patientsEndpoints.Url}{_patientsEndpoints.PatientsEndpoint}{patientId.ToString()}";
		var response = await _httpClient.GetAsync(uri, cancellationToken);
		return await response.Content.ReadFromJsonAsync<PatientResponseDto>(cancellationToken);
	}
}
