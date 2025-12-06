namespace Vinder.IdentityProvider.Domain.Repositories;

public interface ISecretRepository : IBaseRepository<Secret>
{
    public Task<Secret> GetSecretAsync(CancellationToken cancellation = default);
}
