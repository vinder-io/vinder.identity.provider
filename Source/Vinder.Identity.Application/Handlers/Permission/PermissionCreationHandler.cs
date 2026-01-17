namespace Vinder.Identity.Application.Handlers.Permission;

public sealed class PermissionCreationHandler(IPermissionCollection collection, ITenantProvider tenantProvider) :
    IMessageHandler<PermissionCreationScheme, Result<PermissionDetailsScheme>>
{
    public async Task<Result<PermissionDetailsScheme>> HandleAsync(PermissionCreationScheme parameters, CancellationToken cancellation = default)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var filters = PermissionFilters.WithSpecifications()
            .WithName(parameters.Name)
            .Build();

        var permissions = await collection.GetPermissionsAsync(filters, cancellation: cancellation);
        var existingPermission = permissions.FirstOrDefault();

        if (existingPermission is not null)
        {
            return Result<PermissionDetailsScheme>.Failure(PermissionErrors.PermissionAlreadyExists);
        }

        var permission = PermissionMapper.AsPermission(parameters, tenant);
        var createdPermission = await collection.InsertAsync(permission, cancellation: cancellation);

        return Result<PermissionDetailsScheme>.Success(PermissionMapper.AsResponse(createdPermission));
    }
}