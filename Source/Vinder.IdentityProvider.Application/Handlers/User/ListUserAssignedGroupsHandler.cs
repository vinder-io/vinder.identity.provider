namespace Vinder.IdentityProvider.Application.Handlers.User;

public sealed class ListUserAssignedGroupsHandler(IUserRepository repository) :
    IRequestHandler<ListUserAssignedGroups, Result<IReadOnlyCollection<GroupBasicDetails>>>
{
    public async Task<Result<IReadOnlyCollection<GroupBasicDetails>>> Handle(ListUserAssignedGroups request, CancellationToken cancellationToken)
    {
        var filters = new UserFiltersBuilder()
            .WithUserId(request.UserId)
            .Build();

        var users = await repository.GetUsersAsync(filters, cancellationToken);
        var user = users.FirstOrDefault();

        return user is not null
            ? Result<IReadOnlyCollection<GroupBasicDetails>>.Success(GroupMapper.AsBasicResponse(user.Groups))
            : Result<IReadOnlyCollection<GroupBasicDetails>>.Failure(UserErrors.UserDoesNotExist);
    }
}
