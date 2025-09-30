namespace Vinder.IdentityProvider.Domain.Repositories;

public interface IPermissionRepository : IRepository<Permission>
{
    public Task<IReadOnlyCollection<Permission>> GetPermissionsAsync(
        PermissionFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountAsync(
        PermissionFilters filters,
        CancellationToken cancellation = default
    );
}