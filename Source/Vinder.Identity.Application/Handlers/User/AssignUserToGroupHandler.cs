namespace Vinder.Identity.Application.Handlers.User;

public sealed class AssignUserToGroupHandler(IUserCollection userCollection, IGroupCollection groupCollection) :
    IRequestHandler<AssignUserToGroupScheme, Result>
{
    public async Task<Result> Handle(AssignUserToGroupScheme request, CancellationToken cancellationToken)
    {
        var userFilters = new UserFiltersBuilder()
            .WithIdentifier(request.UserId)
            .Build();

        var matchingUsers = await userCollection.GetUsersAsync(userFilters, cancellationToken);
        var existingUser = matchingUsers.FirstOrDefault();

        if (existingUser is null)
        {
            return Result.Failure(UserErrors.UserDoesNotExist);
        }

        var groupFilters = new GroupFiltersBuilder()
            .WithIdentifier(request.GroupId)
            .Build();

        var matchingGroups = await groupCollection.GetGroupsAsync(groupFilters, cancellationToken);
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

        await userCollection.UpdateAsync(existingUser, cancellationToken);

        return Result.Success();
    }
}
