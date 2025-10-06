namespace Vinder.IdentityProvider.Application.Handlers.Group;

public sealed class AssignPermissionToGroupHandler(IGroupRepository groupRepository, IPermissionRepository permissionRepository) :
    IRequestHandler<AssignGroupPermissionScheme, Result<GroupDetailsScheme>>
{
    public async Task<Result<GroupDetailsScheme>> Handle(
        AssignGroupPermissionScheme request, CancellationToken cancellationToken)
    {
        var groupFilters = new GroupFiltersBuilder()
            .WithId(request.GroupId)
            .Build();

        var permissionFilters = new PermissionFiltersBuilder()
            .WithName(request.PermissionName.ToLower())
            .Build();

        var groups = await groupRepository.GetGroupsAsync(groupFilters, cancellation: cancellationToken);
        var group = groups.FirstOrDefault();

        if (group is null)
        {
            return Result<GroupDetailsScheme>.Failure(GroupErrors.GroupDoesNotExist);
        }

        var permissions = await permissionRepository.GetPermissionsAsync(permissionFilters, cancellation: cancellationToken);
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

        await groupRepository.UpdateAsync(group, cancellation: cancellationToken);

        return Result<GroupDetailsScheme>.Success(GroupMapper.AsResponse(group));
    }
}