using Appointment.Core.Email;
using Appointment.Infrastructure.Email.Models;
using Appointment.Infrastructure.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Appointment.Infrastructure.Email;

public class EmailSender : IEmailSender
{
    private readonly IOptions<EmailCredentials> _emailCredentials;

    public EmailSender(IOptions<EmailCredentials> emailConfiguration)
    {
        _emailCredentials = emailConfiguration;
    }

    public async Task SendEmailAsync(MessageRequestDto message, CancellationToken cancellationToken)
    {
        var emailMessage = CreateEmailMessage(message);

        await Send(emailMessage, cancellationToken);
    }

    private MimeMessage CreateEmailMessage(MessageRequestDto message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("", _emailCredentials.Value.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

        return emailMessage;
    }

    private async Task Send(MimeMessage message, CancellationToken cancellationToken)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_emailCredentials.Value.SmtpServer, _emailCredentials.Value.Port, MailKit.Security.SecureSocketOptions.Auto, cancellationToken);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailCredentials.Value.UserName, _emailCredentials.Value.Password, cancellationToken);

                await client.SendAsync(message, cancellationToken);
            }
            finally
            {
                await client.DisconnectAsync(true, cancellationToken);
                client.Dispose();
            }
        }

    }
}
