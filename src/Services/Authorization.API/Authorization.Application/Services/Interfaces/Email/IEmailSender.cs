using Authorization.Application.Utility;

namespace Authorization.Application.Services.Interfaces.Email;

public interface IEmailSender
{
    public Task SendEmailAsync(Message message);
}
