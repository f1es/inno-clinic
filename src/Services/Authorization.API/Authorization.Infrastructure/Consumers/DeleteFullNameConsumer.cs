using Authorization.Core.Repositories;
using MassTransit;
using Shared.Queues.Messages;

namespace Authorization.Infrastructure.Consumers;

public class DeleteFullNameConsumer : IConsumer<DeleteFullNameMessage>
{
	private readonly IAccountRepository _accountRepository;

	public DeleteFullNameConsumer(IAccountRepository accountRepository)
	{
		_accountRepository = accountRepository;
	}

	public async Task Consume(ConsumeContext<DeleteFullNameMessage> context)
	{
		var account = await _accountRepository.GetByIdAsync(context.Message.AccountId, trackChanges: true);

		if (account is null)
		{
			return;
		}

		account.FirstName = null;
		account.LastName = null;
		account.MiddleName = null;

		await _accountRepository.SaveAsync();
	}
}
