namespace Vinder.Identity.Application.Handlers.Permission;

public sealed class PermissionUpdateHandler(IPermissionCollection collection, ITenantProvider tenantProvider) :
    IRequestHandler<PermissionUpdateScheme, Result<PermissionDetailsScheme>>
{
    public async Task<Result<PermissionDetailsScheme>> Handle(PermissionUpdateScheme request, CancellationToken cancellationToken)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var filters = new PermissionFiltersBuilder()
            .WithIdentifier(request.PermissionId)
            .Build();

        var permissions = await collection.GetPermissionsAsync(filters, cancellation: cancellationToken);
        var permission = permissions.FirstOrDefault();

        if (permission is null)
        {
            return Result<PermissionDetailsScheme>.Failure(PermissionErrors.PermissionDoesNotExist);
        }

        permission = PermissionMapper.AsPermission(request, permission, tenant);

        var updatedPermission = await collection.UpdateAsync(permission, cancellation: cancellationToken);

        return Result<PermissionDetailsScheme>.Success(PermissionMapper.AsResponse(updatedPermission));
    }
}