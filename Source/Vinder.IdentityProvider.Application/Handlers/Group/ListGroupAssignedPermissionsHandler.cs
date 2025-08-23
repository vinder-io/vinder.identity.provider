namespace Vinder.IdentityProvider.Application.Handlers.Group;

public sealed class ListGroupAssignedPermissionsHandler(IGroupRepository repository) :
    IRequestHandler<ListGroupAssignedPermissions, Result<IReadOnlyCollection<PermissionDetails>>>
{
    public async Task<Result<IReadOnlyCollection<PermissionDetails>>> Handle(ListGroupAssignedPermissions request, CancellationToken cancellationToken)
    {
        var filters = new GroupFiltersBuilder()
            .WithId(request.GroupId)
            .Build();

        var groups = await repository.GetGroupsAsync(filters, cancellationToken);
        var group = groups.FirstOrDefault();

        return group is not null
            ? Result<IReadOnlyCollection<PermissionDetails>>.Success(PermissionMapper.AsResponse(group.Permissions))
            : Result<IReadOnlyCollection<PermissionDetails>>.Failure(GroupErrors.GroupDoesNotExist);
    }
}
