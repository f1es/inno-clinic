using Grpc.Core;
using GrpcServices;
using Services.Core.Repositories;

namespace Services.Application.Grpc.Services;

public class ServiceGrpService : ServiceGrpcServiceProto.ServiceGrpcServiceProtoBase
{
	private readonly IUnitOfWork _unitOfWork;

	public ServiceGrpService(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public override async Task<ProtoBool> IsServiceExist(IsServiceExistRequest request, ServerCallContext context)
	{
		var serviceId = Guid.Parse(request.ServiceId);
		var service = await _unitOfWork.ServiceRepository.GetByIdAsync(serviceId, context.CancellationToken);

		return service == null ? new ProtoBool() { IsExist = false } : new ProtoBool() { IsExist = true };
	}
}


