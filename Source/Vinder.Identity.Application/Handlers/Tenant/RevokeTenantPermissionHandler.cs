namespace Vinder.Identity.Application.Handlers.Tenant;

public sealed class RevokeTenantPermissionHandler(ITenantCollection tenantCollection, IPermissionCollection permissionCollection) :
    IMessageHandler<RevokeTenantPermissionScheme, Result>
{
    public async Task<Result> HandleAsync(RevokeTenantPermissionScheme parameters, CancellationToken cancellation)
    {
        var permissionFilters = new PermissionFiltersBuilder()
            .WithIdentifier(parameters.PermissionId)
            .Build();

        var tenantFilters = new TenantFiltersBuilder()
            .WithIdentifier(parameters.TenantId)
            .Build();

        var tenants = await tenantCollection.GetTenantsAsync(tenantFilters, cancellation);
        var tenant = tenants.FirstOrDefault();

        var permissions = await permissionCollection.GetPermissionsAsync(permissionFilters, cancellation);
        var permission = permissions.FirstOrDefault();

        if (tenant is null)
        {
            return Result.Failure(TenantErrors.TenantDoesNotExist);
        }

        if (permission is null)
        {
            return Result.Failure(PermissionErrors.PermissionDoesNotExist);
        }

        var permissionToRemove = tenant.Permissions.FirstOrDefault(p => p.Id == permission.Id);
        if (permissionToRemove is null)
        {
            return Result.Failure(TenantErrors.PermissionNotAssigned);
        }

        tenant.Permissions.Remove(permissionToRemove);

        await tenantCollection.UpdateAsync(tenant, cancellation);

        return Result.Success();
    }
}
