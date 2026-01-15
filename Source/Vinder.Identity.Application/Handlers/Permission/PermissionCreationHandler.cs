namespace Vinder.Identity.Application.Handlers.Permission;

public sealed class PermissionCreationHandler(IPermissionRepository repository, ITenantProvider tenantProvider) :
    IRequestHandler<PermissionCreationScheme, Result<PermissionDetailsScheme>>
{
    public async Task<Result<PermissionDetailsScheme>> Handle(PermissionCreationScheme request, CancellationToken cancellationToken)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var filters = new PermissionFiltersBuilder()
            .WithName(request.Name)
            .Build();

        var permissions = await repository.GetPermissionsAsync(filters, cancellation: cancellationToken);
        var existingPermission = permissions.FirstOrDefault();

        if (existingPermission is not null)
        {
            return Result<PermissionDetailsScheme>.Failure(PermissionErrors.PermissionAlreadyExists);
        }

        var permission = PermissionMapper.AsPermission(request, tenant);
        var createdPermission = await repository.InsertAsync(permission, cancellation: cancellationToken);

        return Result<PermissionDetailsScheme>.Success(PermissionMapper.AsResponse(createdPermission));
    }
}