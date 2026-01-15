namespace Vinder.Identity.Application.Handlers.Group;

public sealed class AssignPermissionToGroupHandler(IGroupCollection groupCollection, IPermissionCollection permissionCollection) :
    IMessageHandler<AssignGroupPermissionScheme, Result<GroupDetailsScheme>>
{
    public async Task<Result<GroupDetailsScheme>> HandleAsync(
        AssignGroupPermissionScheme parameters, CancellationToken cancellationToken)
    {
        var groupFilters = new GroupFiltersBuilder()
            .WithIdentifier(parameters.GroupId)
            .Build();

        var permissionFilters = new PermissionFiltersBuilder()
            .WithName(parameters.PermissionName.ToLower())
            .Build();

        var groups = await groupCollection.GetGroupsAsync(groupFilters, cancellation: cancellationToken);
        var group = groups.FirstOrDefault();

        if (group is null)
        {
            return Result<GroupDetailsScheme>.Failure(GroupErrors.GroupDoesNotExist);
        }

        var permissions = await permissionCollection.GetPermissionsAsync(permissionFilters, cancellation: cancellationToken);
        var existingPermission = permissions.FirstOrDefault();

        if (existingPermission is null)
        {
            return Result<GroupDetailsScheme>.Failure(PermissionErrors.PermissionDoesNotExist);
        }

        if (group.Permissions.Any(permission => permission.Name == existingPermission.Name))
        {
            return Result<GroupDetailsScheme>.Failure(GroupErrors.GroupAlreadyHasPermission);
        }

        group.Permissions.Add(existingPermission);

        await groupCollection.UpdateAsync(group, cancellation: cancellationToken);

        return Result<GroupDetailsScheme>.Success(GroupMapper.AsResponse(group));
    }
}