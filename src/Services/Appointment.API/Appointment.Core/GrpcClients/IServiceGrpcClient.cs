namespace Appointment.Core.GrpcClients;

public interface IServiceGrpcClient
{
	Task<bool> IsServiceExist(Guid serviceId, CancellationToken cancellationToken);
}
