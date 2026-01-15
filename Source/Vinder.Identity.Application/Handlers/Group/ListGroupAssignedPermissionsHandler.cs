namespace Vinder.Identity.Application.Handlers.Group;

public sealed class ListGroupAssignedPermissionsHandler(IGroupCollection collection) :
    IRequestHandler<ListGroupAssignedPermissionsParameters, Result<IReadOnlyCollection<PermissionDetailsScheme>>>
{
    public async Task<Result<IReadOnlyCollection<PermissionDetailsScheme>>> Handle(
        ListGroupAssignedPermissionsParameters request, CancellationToken cancellationToken)
    {
        var filters = new GroupFiltersBuilder()
            .WithIdentifier(request.GroupId)
            .Build();

        var groups = await collection.GetGroupsAsync(filters, cancellationToken);
        var group = groups.FirstOrDefault();

        return group is not null
            ? Result<IReadOnlyCollection<PermissionDetailsScheme>>.Success(PermissionMapper.AsResponse(group.Permissions))
            : Result<IReadOnlyCollection<PermissionDetailsScheme>>.Failure(GroupErrors.GroupDoesNotExist);
    }
}
