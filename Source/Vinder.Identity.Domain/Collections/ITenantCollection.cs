namespace Vinder.Identity.Domain.Repositories;

public interface ITenantCollection : IAggregateCollection<Tenant>
{
    public Task<IReadOnlyCollection<Tenant>> GetTenantsAsync(
        TenantFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountAsync(
        TenantFilters filters,
        CancellationToken cancellation = default
    );
}