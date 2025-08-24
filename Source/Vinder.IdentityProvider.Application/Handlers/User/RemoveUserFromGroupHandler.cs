namespace Vinder.IdentityProvider.Application.Handlers.User;

public sealed class RemoveUserFromGroupHandler(IUserRepository userRepository, IGroupRepository groupRepository) :
    IRequestHandler<RemoveUserFromGroup, Result>
{
    public async Task<Result> Handle(RemoveUserFromGroup request, CancellationToken cancellationToken)
    {
        var userFilters = new UserFiltersBuilder()
            .WithUserId(request.UserId)
            .Build();

        var groupFilters = new GroupFiltersBuilder()
            .WithId(request.GroupId)
            .Build();

        var users = await userRepository.GetUsersAsync(userFilters, cancellationToken);
        var user = users.FirstOrDefault();

        var groups = await groupRepository.GetGroupsAsync(groupFilters, cancellationToken);
        var group = groups.FirstOrDefault();

        if (user is null)
        {
            return Result.Failure(UserErrors.UserDoesNotExist);
        }

        if (group is null)
        {
            return Result.Failure(GroupErrors.GroupDoesNotExist);
        }

        #pragma warning disable S1764

        // sonar suggests changing the lambda parameter name to avoid confusion,
        // but we prefer keeping it as 'group' for readability. it does not interfere
        // with the outer variable because C# differentiates the lambda parameter from the argument.

        var groupToRemove = user.Groups.FirstOrDefault(group => group.Id == group.Id);
        if (groupToRemove is null)
        {
            return Result.Failure(UserErrors.UserNotInGroup);
        }

        user.Groups.Remove(groupToRemove);

        await userRepository.UpdateAsync(user, cancellationToken);

        return Result.Success();
    }
}
