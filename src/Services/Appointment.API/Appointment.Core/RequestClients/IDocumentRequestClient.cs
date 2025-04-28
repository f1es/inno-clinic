namespace Appointment.Core.RequestClients;

public interface IDocumentRequestClient
{
	public Task CreateDocumentAsync(MemoryStream documentStream);
}
