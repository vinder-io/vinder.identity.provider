namespace Vinder.IdentityProvider.Application.Handlers.Group;

public sealed class AssignPermissionToGroupHandler(IGroupRepository groupRepository, IPermissionRepository permissionRepository) :
    IRequestHandler<AssignGroupPermission, Result<GroupDetails>>
{
    public async Task<Result<GroupDetails>> Handle(AssignGroupPermission request, CancellationToken cancellationToken)
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
            return Result<GroupDetails>.Failure(GroupErrors.GroupDoesNotExist);
        }

        var permissions = await permissionRepository.GetPermissionsAsync(permissionFilters, cancellation: cancellationToken);
        var existingPermission = permissions.FirstOrDefault();

        if (existingPermission is null)
        {
            return Result<GroupDetails>.Failure(PermissionErrors.PermissionDoesNotExist);
        }

        if (group.Permissions.Any(permission => permission.Name == existingPermission.Name))
        {
            return Result<GroupDetails>.Failure(GroupErrors.GroupAlreadyHasPermission);
        }

        group.Permissions.Add(existingPermission);

        await groupRepository.UpdateAsync(group, cancellation: cancellationToken);

        return Result<GroupDetails>.Success(GroupMapper.AsResponse(group));
    }
}