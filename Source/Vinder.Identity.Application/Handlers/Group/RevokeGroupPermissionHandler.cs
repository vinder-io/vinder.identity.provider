namespace Vinder.Identity.Application.Handlers.Group;

public sealed class RevokeGroupPermissionHandler(IGroupCollection groupCollection, IPermissionCollection permissionCollection) :
    IRequestHandler<RevokeGroupPermissionScheme, Result>
{
    public async Task<Result> Handle(RevokeGroupPermissionScheme request, CancellationToken cancellationToken)
    {
        var permissionFilters = new PermissionFiltersBuilder()
            .WithIdentifier(request.PermissionId)
            .Build();

        var groupFilters = new GroupFiltersBuilder()
            .WithIdentifier(request.GroupId)
            .Build();

        var groups = await groupCollection.GetGroupsAsync(groupFilters, cancellationToken);
        var group = groups.FirstOrDefault();

        var permissions = await permissionCollection.GetPermissionsAsync(permissionFilters, cancellationToken);
        var permission = permissions.FirstOrDefault();

        if (group is null)
        {
            return Result.Failure(GroupErrors.GroupDoesNotExist);
        }

        if (permission is null)
        {
            return Result.Failure(PermissionErrors.PermissionDoesNotExist);
        }

        #pragma warning disable S1764

        // sonar suggests changing the lambda parameter name to avoid confusion,
        // but we prefer keeping it as 'permission' for readability. it does not interfere
        // with the outer variable because C# differentiates the lambda parameter from the argument.

        var permissionToRemove = group.Permissions.FirstOrDefault(permission => permission.Id == permission.Id);
        if (permissionToRemove is null)
        {
            return Result.Failure(GroupErrors.PermissionNotAssigned);
        }

        group.Permissions.Remove(permissionToRemove);

        await groupCollection.UpdateAsync(group, cancellationToken);

        return Result.Success();
    }
}