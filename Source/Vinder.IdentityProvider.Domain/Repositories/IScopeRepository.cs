namespace Vinder.IdentityProvider.Domain.Repositories;

public interface IScopeRepository : IRepository<Scope>
{
    public Task<IReadOnlyCollection<Scope>> GetScopesAsync(
        ScopeFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountAsync(
        ScopeFilters filters,
        CancellationToken cancellation = default
    );
}