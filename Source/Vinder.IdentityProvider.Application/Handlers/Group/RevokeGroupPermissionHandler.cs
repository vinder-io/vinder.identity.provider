namespace Vinder.IdentityProvider.Application.Handlers.Group;

public sealed class RevokeGroupPermissionHandler(IGroupRepository groupRepository, IPermissionRepository permissionRepository) :
    IRequestHandler<RevokeGroupPermission, Result>
{
    public async Task<Result> Handle(RevokeGroupPermission request, CancellationToken cancellationToken)
    {
        var permissionFilters = new PermissionFiltersBuilder()
            .WithPermissionId(request.PermissionId)
            .Build();

        var groupFilters = new GroupFiltersBuilder()
            .WithId(request.GroupId)
            .Build();

        var groups = await groupRepository.GetGroupsAsync(groupFilters, cancellationToken);
        var group = groups.FirstOrDefault();

        var permissions = await permissionRepository.GetPermissionsAsync(permissionFilters, cancellationToken);
        var permission = permissions.FirstOrDefault();

        if (group is null)
        {
            return Result.Failure(GroupErrors.GroupDoesNotExist);
        }

        if (permission is null)
        {
            return Result.Failure(PermissionErrors.PermissionDoesNotExist);
        }

        #pragma warning disable S1125

        // sonar suggests using "!" instead of "is false",
        // but we prefer "is false" because it makes the intent more readable.

        var permissionIsRemoved = group.Permissions.Remove(permission);
        if (permissionIsRemoved is false)
        {
            return Result.Failure(GroupErrors.PermissionNotAssigned);
        }

        await groupRepository.UpdateAsync(group, cancellationToken);

        return Result.Success();
    }
}