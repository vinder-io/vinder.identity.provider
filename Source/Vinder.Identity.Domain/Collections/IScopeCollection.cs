namespace Vinder.Identity.Domain.Repositories;

public interface IScopeRepository : IAggregateCollection<Scope>
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