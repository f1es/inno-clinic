using System.Threading;

namespace Appointment.Core.RequestClients;

public interface IServicesRequestClient
{
	public Task<bool> IsServiceExistAsync(Guid serviceId, CancellationToken cancellationToken);
}
