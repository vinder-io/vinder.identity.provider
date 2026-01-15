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

        #pragma warning disable S1764

        // sonar suggests changing the lambda parameter name to avoid confusion,
        // but we prefer keeping it as 'permission' for readability. it does not interfere
        // with the outer variable because C# differentiates the lambda parameter from the argument.

        var permissionToRemove = tenant.Permissions.FirstOrDefault(permission => permission.Id == permission.Id);
        if (permissionToRemove is null)
        {
            return Result.Failure(TenantErrors.PermissionNotAssigned);
        }

        tenant.Permissions.Remove(permissionToRemove);

        await tenantCollection.UpdateAsync(tenant, cancellation);

        return Result.Success();
    }
}
