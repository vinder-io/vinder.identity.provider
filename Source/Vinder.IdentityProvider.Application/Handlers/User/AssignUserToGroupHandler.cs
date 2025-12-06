namespace Vinder.IdentityProvider.Application.Handlers.User;

public sealed class AssignUserToGroupHandler(IUserRepository userRepository, IGroupRepository groupRepository) :
    IRequestHandler<AssignUserToGroupScheme, Result>
{
    public async Task<Result> Handle(AssignUserToGroupScheme request, CancellationToken cancellationToken)
    {
        var userFilters = new UserFiltersBuilder()
            .WithIdentifier(request.UserId)
            .Build();

        var matchingUsers = await userRepository.GetUsersAsync(userFilters, cancellationToken);
        var existingUser = matchingUsers.FirstOrDefault();

        if (existingUser is null)
        {
            return Result.Failure(UserErrors.UserDoesNotExist);
        }

        var groupFilters = new GroupFiltersBuilder()
            .WithIdentifier(request.GroupId)
            .Build();

        var matchingGroups = await groupRepository.GetGroupsAsync(groupFilters, cancellationToken);
        var existingGroup = matchingGroups.FirstOrDefault();

        if (existingGroup is null)
        {
            return Result.Failure(GroupErrors.GroupDoesNotExist);
        }

        if (existingUser.Groups.Any(group => group.Id == request.GroupId))
        {
            return Result.Failure(UserErrors.UserAlreadyInGroup);
        }

        existingUser.Groups.Add(existingGroup);

        var groupPermissions = existingGroup.Permissions ?? [  ];
        var userPermissions = existingUser.Permissions ?? [  ];

        var newPermissions = groupPermissions
            .Where(permission => userPermissions.All(existingPermission => existingPermission.Name != permission.Name))
            .ToList();

        existingUser.Permissions = userPermissions
            .Concat(newPermissions)
            .ToList();

        await userRepository.UpdateAsync(existingUser, cancellationToken);

        return Result.Success();
    }
}
