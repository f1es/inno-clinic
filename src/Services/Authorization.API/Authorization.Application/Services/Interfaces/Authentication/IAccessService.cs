using Authorization.Application.Utility;
using Authorization.Core.Dto.Request;

namespace Authorization.Application.Services.Interfaces.Authentication;

public interface IAccessService
{
    public Task<Tokens> LoginAsync(LoginAccountRequestDto loginAccountRequestDto);
    public Task<Tokens> RefreshAsync(Tokens tokens);
}
