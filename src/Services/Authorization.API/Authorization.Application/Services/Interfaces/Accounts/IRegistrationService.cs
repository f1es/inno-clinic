using Authorization.Core.Dto.Request;

namespace Authorization.Application.Services.Interfaces.Accounts;

public interface IRegistrationService
{
    public Task RegisterAsync(RegisterAccountRequestDto registerAccountRequestDto);
}
