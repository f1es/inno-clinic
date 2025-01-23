using Authorization.Core.Dto.Request;

namespace Authorization.Application.Services.Interfaces;

public interface IAccountService
{
	public Task RegisterAsync(RegisterAccountRequestDto registerAccountRequestDto);
	public Task<string> LoginAsync(LoginAccountRequestDto loginAccountRequestDto);
}
