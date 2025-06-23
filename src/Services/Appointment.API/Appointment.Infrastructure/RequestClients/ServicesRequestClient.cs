using Appointment.Core.RequestClients;
using Appointment.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Shared.Options;

namespace Appointment.Infrastructure.RequestClients;

public class ServicesRequestClient : IServicesRequestClient
{
	private readonly ServicesEndpointsOptions _servicesEndpoints;
	private readonly HttpClient _httpClient;

	public ServicesRequestClient(IOptions<ServicesEndpointsOptions> servicesEndpoints, HttpClient httpClient)
	{
		_servicesEndpoints = servicesEndpoints.Value;
		_httpClient = httpClient;
	}

	public async Task<bool> IsServiceExistAsync(Guid serviceId, CancellationToken cancellationToken)
	{
		var uri = $"{_servicesEndpoints.Url}{_servicesEndpoints.ServicesEndpoint}{serviceId.ToString()}";
		var response = await _httpClient.GetAsync(uri, cancellationToken);
		return response.IsSuccessStatusCode;
	}
}
