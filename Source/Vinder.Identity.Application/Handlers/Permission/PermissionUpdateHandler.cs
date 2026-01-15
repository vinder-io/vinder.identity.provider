namespace Vinder.Identity.Application.Handlers.Permission;

public sealed class PermissionUpdateHandler(IPermissionCollection collection, ITenantProvider tenantProvider) :
    IMessageHandler<PermissionUpdateScheme, Result<PermissionDetailsScheme>>
{
    public async Task<Result<PermissionDetailsScheme>> HandleAsync(PermissionUpdateScheme parameters, CancellationToken cancellation)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var filters = new PermissionFiltersBuilder()
            .WithIdentifier(parameters.PermissionId)
            .Build();

        var permissions = await collection.GetPermissionsAsync(filters, cancellation: cancellation);
        var permission = permissions.FirstOrDefault();

        if (permission is null)
        {
            return Result<PermissionDetailsScheme>.Failure(PermissionErrors.PermissionDoesNotExist);
        }

        permission = PermissionMapper.AsPermission(parameters, permission, tenant);

        var updatedPermission = await collection.UpdateAsync(permission, cancellation: cancellation);

        return Result<PermissionDetailsScheme>.Success(PermissionMapper.AsResponse(updatedPermission));
    }
}