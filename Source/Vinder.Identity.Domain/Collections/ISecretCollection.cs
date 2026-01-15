namespace Vinder.Identity.Domain.Repositories;

public interface ISecretCollection : IAggregateCollection<Secret>
{
    public Task<Secret> GetSecretAsync(CancellationToken cancellation = default);
}
