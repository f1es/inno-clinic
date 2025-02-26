using Appointment.Core.RequestClients;
using Appointment.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Appointment.Infrastructure.RequestClients;

public class ServicesRequestClient : IServicesRequestClient
{
	private readonly ServicesEndpoints _servicesEndpoints;
	private readonly HttpClient _httpClient;

	public ServicesRequestClient(IOptions<ServicesEndpoints> servicesEndpoints, HttpClient httpClient)
	{
		_servicesEndpoints = servicesEndpoints.Value;
		_httpClient = httpClient;
	}

	public async Task<bool> IsServiceExistAsync(Guid serviceId, CancellationToken cancellationToken)
	{
		var uri = $"{_servicesEndpoints.Url}{_servicesEndpoints.IsServiceExist}{serviceId.ToString()}";
		var response = await _httpClient.GetAsync(uri, cancellationToken);
		return response.IsSuccessStatusCode ? true : false;
	}
}
