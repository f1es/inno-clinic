using Appointment.Core.Dto.Response;
using Appointment.Core.RequestClients;
using Appointment.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace Appointment.Infrastructure.RequestClients;

public class AccountRequestClient : IAccountRequestClient
{
	private readonly AccountsEndpoints _accountsEndpoints;
	private readonly HttpClient _httpClient;

	public AccountRequestClient(IOptions<AccountsEndpoints> accountsEndpoints, HttpClient httpClient)
	{
		_accountsEndpoints = accountsEndpoints.Value;
		_httpClient = httpClient;
	}

	public async Task<AccountResponseDto> GetAccountAsync(Guid accountId, CancellationToken cancellationToken)
	{
		var uri = $"{_accountsEndpoints.Url}{_accountsEndpoints.AccountsEndpoint}{accountId.ToString()}";
		var response = await _httpClient.GetAsync(uri, cancellationToken);
		response = await HandleResponseAsync(response);

		return await response.Content.ReadFromJsonAsync<AccountResponseDto>(cancellationToken);
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
