using Authorization.Core.Dto.Request;

namespace Authorization.Application.Services.Interfaces;

public interface IAccessService
{
	public Task RegisterAsync(RegisterAccountRequestDto registerAccountRequestDto);
	public Task<string> LoginAsync(LoginAccountRequestDto loginAccountRequestDto);
	public Task VerifyEmailAsync(string token);
}
