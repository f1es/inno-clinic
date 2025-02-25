using Appointment.Core.GrpcClients;
using GrpcServices;

namespace Appointment.Infrastructure.GrpcClients;

public class ServiceGrpcClient : IServiceGrpcClient
{
	private readonly ServiceGrpcServiceProto.ServiceGrpcServiceProtoClient _client;

	public ServiceGrpcClient(ServiceGrpcServiceProto.ServiceGrpcServiceProtoClient client)
	{
		_client = client;
	}

	public async Task<bool> IsServiceExist(Guid serviceId, CancellationToken cancellationToken)
	{
		var request = new IsServiceExistRequest() { ServiceId = serviceId.ToString() };

		var protoBool = await _client.IsServiceExistAsync(request, cancellationToken: cancellationToken);

		return protoBool.IsExist;
	}
}
