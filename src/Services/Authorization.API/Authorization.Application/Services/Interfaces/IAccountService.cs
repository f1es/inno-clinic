using Authorization.Core.Dto.Request;
using Authorization.Core.Dto.Response;

namespace Authorization.Application.Services.Interfaces;

public interface IAccountService
{
	public Task<IEnumerable<AccountResponseDto>> GetAllAsync();
	public Task<AccountResponseDto> GetByIdAsync(Guid id);
	public Task UpdateAsync(Guid id, UpdateAccountRequestDto updateAccountRequestDto);
	public Task DeleteAsync(Guid id);
}
