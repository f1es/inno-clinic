using Authorization.Application.Utility;
using Authorization.Core.Dto.Request;

namespace Authorization.Application.Services.Interfaces;

public interface IAccessService
{
	public Task RegisterAsync(RegisterAccountRequestDto registerAccountRequestDto);
	public Task<Tokens> LoginAsync(LoginAccountRequestDto loginAccountRequestDto);
	public Task VerifyEmailAsync(string token);
	public Task<Tokens> RefreshAsync(Tokens tokens);
}
