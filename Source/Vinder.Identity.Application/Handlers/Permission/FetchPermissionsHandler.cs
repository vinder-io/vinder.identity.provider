namespace Vinder.Identity.Application.Handlers.Permission;

public sealed class FetchPermissionsHandler(IPermissionRepository repository) :
    IRequestHandler<PermissionsFetchParameters, Result<Pagination<PermissionDetailsScheme>>>
{
    public async Task<Result<Pagination<PermissionDetailsScheme>>> Handle(
        PermissionsFetchParameters parameters, CancellationToken cancellationToken)
    {
        var filters = PermissionMapper.AsFilters(parameters);

        var permissions = await repository.GetPermissionsAsync(filters, cancellation: cancellationToken);
        var totalPermission = await repository.CountAsync(filters, cancellation: cancellationToken);

        var pagination = new Pagination<PermissionDetailsScheme>
        {
            Items = [.. permissions.Select(permission => PermissionMapper.AsResponse(permission))],
            Total = (int)totalPermission,
            PageNumber = parameters.Pagination?.PageNumber ?? 1,
            PageSize = parameters.Pagination?.PageSize ?? 20
        };

        return Result<Pagination<PermissionDetailsScheme>>.Success(pagination);
    }
}
