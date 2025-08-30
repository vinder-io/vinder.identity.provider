namespace Vinder.IdentityProvider.Application.Handlers.Permission;

public sealed class PermissionUpdateHandler(IPermissionRepository repository, ITenantProvider tenantProvider) :
    IRequestHandler<PermissionForUpdate, Result<PermissionDetails>>
{
    public async Task<Result<PermissionDetails>> Handle(PermissionForUpdate request, CancellationToken cancellationToken)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var filters = new PermissionFiltersBuilder()
            .WithPermissionId(request.PermissionId)
            .Build();

        var permissions = await repository.GetPermissionsAsync(filters, cancellation: cancellationToken);
        var permission = permissions.FirstOrDefault();

        if (permission is null)
        {
            return Result<PermissionDetails>.Failure(PermissionErrors.PermissionDoesNotExist);
        }

        permission = PermissionMapper.AsPermission(request, permission, tenant);

        var updatedPermission = await repository.UpdateAsync(permission, cancellation: cancellationToken);

        return Result<PermissionDetails>.Success(PermissionMapper.AsResponse(updatedPermission));
    }
}