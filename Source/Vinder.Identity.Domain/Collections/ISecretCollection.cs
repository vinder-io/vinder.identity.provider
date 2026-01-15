namespace Vinder.Identity.Domain.Collections;

public interface ISecretCollection : IAggregateCollection<Secret>
{
    public Task<Secret> GetSecretAsync(CancellationToken cancellation = default);
}
