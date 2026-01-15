namespace Vinder.Identity.Application.Handlers.User;

public sealed class RemoveUserFromGroupHandler(IUserCollection userCollection, IGroupCollection groupCollection) :
    IRequestHandler<RemoveUserFromGroupScheme, Result>
{
    public async Task<Result> Handle(RemoveUserFromGroupScheme request, CancellationToken cancellationToken)
    {
        var userFilters = new UserFiltersBuilder()
            .WithIdentifier(request.UserId)
            .Build();

        var groupFilters = new GroupFiltersBuilder()
            .WithIdentifier(request.GroupId)
            .Build();

        var users = await userCollection.GetUsersAsync(userFilters, cancellationToken);
        var user = users.FirstOrDefault();

        var groups = await groupCollection.GetGroupsAsync(groupFilters, cancellationToken);
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

        await userCollection.UpdateAsync(user, cancellationToken);

        return Result.Success();
    }
}
