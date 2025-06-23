using Appointment.Core.Dto.Response;
using Appointment.Core.RequestClients;
using Microsoft.Extensions.Options;
using Shared.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace Appointment.Infrastructure.RequestClients;

public class PatientsRequestClient : IPatientsRequestClient
{
	private readonly PatientsEndpointsOptions _patientsEndpoints;
	private readonly HttpClient _httpClient;

	public PatientsRequestClient(IOptions<PatientsEndpointsOptions> servicesEndpoints, HttpClient httpClient)
	{
		_patientsEndpoints = servicesEndpoints.Value;
		_httpClient = httpClient;
	}

	public async Task<PatientResponseDto> GetPatientAsync(Guid patientId, CancellationToken cancellationToken)
	{
		var uri = $"{_patientsEndpoints.Url}{_patientsEndpoints.PatientsEndpoint}{patientId.ToString()}";
		var response = await _httpClient.GetAsync(uri, cancellationToken);
		response = await HandleResponseAsync(response);

		return await response.Content.ReadFromJsonAsync<PatientResponseDto>(cancellationToken);
	}

	private async Task<HttpResponseMessage> HandleResponseAsync(HttpResponseMessage response)
	{
		if (!response.IsSuccessStatusCode)
		{
			var responseMessage = await response.Content.ReadAsStringAsync();
			var responseJson = JsonSerializer.Deserialize<JsonElement>(responseMessage);
			var errorMessage = responseJson.GetProperty("error");

			throw new Exception(errorMessage.ToString());
		}

		return response;
	}
}
