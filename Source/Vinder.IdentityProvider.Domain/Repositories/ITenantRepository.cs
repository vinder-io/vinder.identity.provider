namespace Vinder.IdentityProvider.Domain.Repositories;

public interface ITenantRepository : IRepository<Tenant>
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