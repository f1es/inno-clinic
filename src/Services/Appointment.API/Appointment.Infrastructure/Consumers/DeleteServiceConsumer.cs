using Appointment.Core.Repositories;
using MassTransit;
using Shared.Queues.Messages;

namespace Appointment.Infrastructure.Consumers;

public class DeleteServiceConsumer : IConsumer<DeleteServiceMessage>
{
    private readonly IUnitOfWork _unitOfWork;

	public DeleteServiceConsumer(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task Consume(ConsumeContext<DeleteServiceMessage> context)
	{
		await _unitOfWork.AppointmentRepository.RemoveServiceIdAsync(context.Message.Id);
	}
}
