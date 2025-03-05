using Appointment.Core.RequestClients;

namespace Appointment.IntegrationTests.Utility;

public class ServicesRequestClientMock : IServicesRequestClient
{
	public Task<bool> IsServiceExistAsync(Guid serviceId, CancellationToken cancellationToken)
	{
		return Task.FromResult(true);
	}
}