using Shared.Queues.Messages;

namespace Profiles.Core.Publishers;

public interface IFullNamePublisher
{
    public Task PublishUpdateAsync(UpdateFullNameMessage updateFullNameMessage, CancellationToken cancellationToken);
    public Task PublishDeleteAsync(DeleteFullNameMessage deleteFullNameMessage, CancellationToken cancellationToken);
}
