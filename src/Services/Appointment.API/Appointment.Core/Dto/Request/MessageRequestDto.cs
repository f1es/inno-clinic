using MimeKit;

namespace Appointment.Infrastructure.Email.Models;

public class MessageRequestDto
{
	public List<MailboxAddress> To { get; set; }
	public string Subject { get; set; }
	public string Content { get; set; }

	public MessageRequestDto(IEnumerable<string> to, string subject, string content)
	{
		To = new List<MailboxAddress>();

		To.AddRange(to.Select(x => new MailboxAddress("", x)));
		Subject = subject;
		Content = content;
	}
}
