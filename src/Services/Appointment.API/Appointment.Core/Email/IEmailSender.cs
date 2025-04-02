using Appointment.Infrastructure.Email.Models;

namespace Appointment.Core.Email;

public interface IEmailSender
{
    public Task SendEmailAsync(MessageRequestDto message, CancellationToken cancellationToken);
}
