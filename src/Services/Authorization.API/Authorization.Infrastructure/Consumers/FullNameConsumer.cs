using Authorization.Core.Repositories;
using MassTransit;
using Shared.Queues.Messages;

namespace Authorization.Infrastructure.Consumers;

public class FullNameConsumer : IConsumer<DeleteFullNameMessage>, IConsumer<UpdateFullNameMessage>
{
	private readonly IAccountRepository _accountRepository;

	public FullNameConsumer(IAccountRepository accountRepository)
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

	public async Task Consume(ConsumeContext<UpdateFullNameMessage> context)
	{
		var account = await _accountRepository.GetByIdAsync(context.Message.AccountId, trackChanges: true);

		if (account is null)
		{
			return;
		}

		account.FirstName = context.Message.FirstName;
		account.LastName = context.Message.LastName;
		account.MiddleName = context.Message.MiddleName;

		await _accountRepository.SaveAsync();
	}
}
