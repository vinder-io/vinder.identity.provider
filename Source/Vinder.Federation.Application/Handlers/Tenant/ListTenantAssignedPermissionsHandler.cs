namespace Vinder.Federation.Application.Handlers.Tenant;

public sealed class ListTenantAssignedPermissionsHandler(ITenantCollection collection) :
    IMessageHandler<ListTenantAssignedPermissionsParameters, Result<IReadOnlyCollection<PermissionDetailsScheme>>>
{
    public async Task<Result<IReadOnlyCollection<PermissionDetailsScheme>>> HandleAsync(
        ListTenantAssignedPermissionsParameters parameters, CancellationToken cancellation = default)
    {
        var filters = TenantFilters.WithSpecifications()
            .WithIdentifier(parameters.TenantId)
            .Build();

        var tenants = await collection.GetTenantsAsync(filters, cancellation);
        var tenant = tenants.FirstOrDefault();

        return tenant is not null
            ? Result<IReadOnlyCollection<PermissionDetailsScheme>>.Success(PermissionMapper.AsResponse(tenant.Permissions))
            : Result<IReadOnlyCollection<PermissionDetailsScheme>>.Failure(TenantErrors.TenantDoesNotExist);
    }
}
