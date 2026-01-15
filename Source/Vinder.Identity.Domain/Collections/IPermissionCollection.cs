namespace Vinder.Identity.Domain.Collections;

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