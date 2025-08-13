namespace Vinder.IdentityProvider.Application.Handlers.Permission;

public sealed class PermissionCreationHandler(IPermissionRepository repository, ITenantProvider tenantProvider) :
    IRequestHandler<PermissionForCreation, Result<PermissionDetails>>
{
    public async Task<Result<PermissionDetails>> Handle(PermissionForCreation request, CancellationToken cancellationToken)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var filters = new PermissionFiltersBuilder()
            .WithName(request.Name)
            .Build();

        var permissions = await repository.GetPermissionsAsync(filters, cancellation: cancellationToken);
        var existingPermission = permissions.FirstOrDefault();

        if (existingPermission is not null)
        {
            return Result<PermissionDetails>.Failure(PermissionErrors.PermissionAlreadyExists);
        }

        var permission = PermissionMapper.AsPermission(request, tenant);
        var createdPermission = await repository.InsertAsync(permission, cancellation: cancellationToken);

        return Result<PermissionDetails>.Success(PermissionMapper.AsResponse(createdPermission));
    }
}