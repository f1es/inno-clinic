using Appointment.Core.Dto.Response;

namespace Appointment.Core.RequestClients;

public interface IAccountRequestClient
{
	public Task<AccountResponseDto> GetAccountAsync(Guid accountId, CancellationToken cancellationToken);
}
