namespace Vinder.IdentityProvider.Application.Handlers.Permission;

public sealed class FetchPermissionsHandler(IPermissionRepository repository) :
    IRequestHandler<PermissionsFetchParameters, Result<Pagination<PermissionDetailsScheme>>>
{
    public async Task<Result<Pagination<PermissionDetailsScheme>>> Handle(PermissionsFetchParameters request, CancellationToken cancellationToken)
    {
        var filters = PermissionMapper.AsFilters(request);

        var permissions = await repository.GetPermissionsAsync(filters, cancellation: cancellationToken);
        var totalPermission = await repository.CountAsync(filters, cancellation: cancellationToken);

        var pagination = new Pagination<PermissionDetailsScheme>
        {
            Items = [.. permissions.Select(permission => PermissionMapper.AsResponse(permission))],
            Total = (int)totalPermission,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };

        return Result<Pagination<PermissionDetailsScheme>>.Success(pagination);
    }
}
