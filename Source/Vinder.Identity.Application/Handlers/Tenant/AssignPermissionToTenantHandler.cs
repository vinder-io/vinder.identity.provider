namespace Vinder.Identity.Application.Handlers.Tenant;

public sealed class AssignPermissionToTenantHandler(ITenantCollection tenantCollection, IPermissionCollection permissionCollection) :
    IMessageHandler<AssignTenantPermissionScheme, Result<IReadOnlyCollection<PermissionDetailsScheme>>>
{
    public async Task<Result<IReadOnlyCollection<PermissionDetailsScheme>>> HandleAsync(
        AssignTenantPermissionScheme parameters, CancellationToken cancellationToken)
    {
        var tenantFilters = new TenantFiltersBuilder()
            .WithIdentifier(parameters.TenantId)
            .Build();

        var permissionFilters = new PermissionFiltersBuilder()
            .WithName(parameters.PermissionName.ToLower())
            .Build();

        var tenants = await tenantCollection.GetTenantsAsync(tenantFilters, cancellation: cancellationToken);
        var tenant = tenants.FirstOrDefault();

        if (tenant is null)
        {
            return Result<IReadOnlyCollection<PermissionDetailsScheme>>.Failure(TenantErrors.TenantDoesNotExist);
        }

        var permissions = await permissionCollection.GetPermissionsAsync(permissionFilters, cancellation: cancellationToken);
        var existingPermission = permissions.FirstOrDefault();

        if (existingPermission is null)
        {
            return Result<IReadOnlyCollection<PermissionDetailsScheme>>.Failure(PermissionErrors.PermissionDoesNotExist);
        }

        if (tenant.Permissions.Any(permission => permission.Name == existingPermission.Name))
        {
            return Result<IReadOnlyCollection<PermissionDetailsScheme>>.Failure(TenantErrors.TenantAlreadyHasPermission);
        }

        tenant.Permissions.Add(existingPermission);

        await tenantCollection.UpdateAsync(tenant, cancellation: cancellationToken);

        return Result<IReadOnlyCollection<PermissionDetailsScheme>>.Success(PermissionMapper.AsResponse(tenant.Permissions));
    }
}
