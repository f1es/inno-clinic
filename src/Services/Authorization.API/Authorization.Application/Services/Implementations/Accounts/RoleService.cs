using Authorization.Application.Services.Interfaces.Accounts;
using Authorization.Application.Utility;
using Authorization.Core.Dto.Request;
using Authorization.Core.Models;
using Authorization.Core.Repositories;
using Shared.Exceptions;
using System.Data;

namespace Authorization.Application.Services.Implementations.Accounts;

public class RoleService : IRoleService
{
	private readonly IAccountRepository _accountRepository;

	public RoleService(IAccountRepository accountRepository)
	{
		_accountRepository = accountRepository;
	}

	public async Task GrantRoleAsync(Guid accountId, UpdateRoleRequestDto updateRoleRequestDto)
	{
		var role = updateRoleRequestDto.Role;

		if (!Roles.AllRoles.Contains(role) && role != null)
		{
			throw new BadRequestException($"Incorrect role {role}");
		}

		var account = await _accountRepository.GetByIdAsync(accountId, trackChanges: true);

		if (account == null)
		{
			throw new NotFoundException(nameof(account), accountId);
		}

		account.Role = role;
		await _accountRepository.SaveAsync();
	}
}
