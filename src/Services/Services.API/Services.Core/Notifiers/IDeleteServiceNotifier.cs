namespace Services.Core.Notifiers;

public interface IDeleteServiceNotifier
{
	public Task NotifyAsync(Guid serviceId, CancellationToken cancellationToken);
}
