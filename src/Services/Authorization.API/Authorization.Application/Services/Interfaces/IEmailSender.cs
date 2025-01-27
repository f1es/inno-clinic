using Authorization.Application.Utility;

namespace Authorization.Application.Services.Interfaces;

public interface IEmailSender
{
	public Task SendEmailAsync(Message message);
}
