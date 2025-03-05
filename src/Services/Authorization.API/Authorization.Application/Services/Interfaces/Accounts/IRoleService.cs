using Authorization.Core.Dto.Request;

namespace Authorization.Application.Services.Interfaces.Accounts;

public interface IRoleService
{
	public Task GrantRoleAsync(Guid accountId, UpdateRoleRequestDto updateRoleRequestDto);
}