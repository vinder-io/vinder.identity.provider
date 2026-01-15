namespace Vinder.Identity.Domain.Collections;

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