namespace Vinder.IdentityProvider.Domain.Repositories;

public interface ISecretRepository : IRepository<Secret>
{
    public Task<Secret> GetSecretAsync(CancellationToken cancellation = default);
}