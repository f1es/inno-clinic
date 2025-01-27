using Authorization.Application.Configuration;
using Authorization.Application.Utility;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Authorization.Application.Services.Implementations;

public class EmailSender : Interfaces.IEmailSender
{
	private readonly IOptions<EmailOptions> _emailConfiguration;

	public EmailSender(IOptions<EmailOptions> emailConfiguration)
	{
		_emailConfiguration = emailConfiguration;
	}

	public async Task SendEmailAsync(Message message)
	{
		var emailMessage = CreateEmailMessage(message);

		await Send(emailMessage);
	}

	private MimeMessage CreateEmailMessage(Message message)
	{
		var emailMessage = new MimeMessage();
		emailMessage.From.Add(new MailboxAddress("", _emailConfiguration.Value.From));
		emailMessage.To.AddRange(message.To);
		emailMessage.Subject = message.Subject;
		emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

		return emailMessage;
	}

	private async Task Send(MimeMessage message)
	{
		using (var client = new SmtpClient())
		{
			try
			{
				await client.ConnectAsync(_emailConfiguration.Value.SmtpServer, _emailConfiguration.Value.Port);
				client.AuthenticationMechanisms.Remove("XOAUTH2");
				await client.AuthenticateAsync(_emailConfiguration.Value.UserName, _emailConfiguration.Value.Password);

				await client.SendAsync(message);
			}
			finally
			{
				await client.DisconnectAsync(true);
				client.Dispose();
			}
		}
		
	} 
}
