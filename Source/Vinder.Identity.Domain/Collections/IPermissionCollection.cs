namespace Vinder.Identity.Domain.Repositories;

public interface IPermissionRepository : IAggregateCollection<Permission>
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