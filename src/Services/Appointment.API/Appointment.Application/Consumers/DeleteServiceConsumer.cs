using Appointment.Core.Repositories;
using MassTransit;
using Shared.Messages;

namespace Appointment.Application.Consumers;

public class DeleteServiceConsumer : IConsumer<DeleteServiceMessage>
{
    private readonly IUnitOfWork _unitOfWork;

	public DeleteServiceConsumer(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task Consume(ConsumeContext<DeleteServiceMessage> context)
	{
		await _unitOfWork.AppointmentRepository.RemoveServiceId(context.Message.Id);
	}
}
