using Appointment.Core.Dto.Response;
using Appointment.Core.RequestClients;
using Appointment.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

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
		return await response.Content.ReadFromJsonAsync<AccountResponseDto>(cancellationToken);
	}
}
