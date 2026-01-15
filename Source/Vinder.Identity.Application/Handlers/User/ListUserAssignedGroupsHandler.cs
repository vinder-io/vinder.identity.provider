namespace Vinder.Identity.Application.Handlers.User;

public sealed class ListUserAssignedGroupsHandler(IUserCollection collection) :
    IRequestHandler<ListUserAssignedGroupsParameters, Result<IReadOnlyCollection<GroupBasicDetailsScheme>>>
{
    public async Task<Result<IReadOnlyCollection<GroupBasicDetailsScheme>>> Handle(
        ListUserAssignedGroupsParameters request, CancellationToken cancellationToken)
    {
        var filters = new UserFiltersBuilder()
            .WithIdentifier(request.UserId)
            .Build();

        var users = await collection.GetUsersAsync(filters, cancellationToken);
        var user = users.FirstOrDefault();

        return user is not null
            ? Result<IReadOnlyCollection<GroupBasicDetailsScheme>>.Success(GroupMapper.AsBasicResponse(user.Groups))
            : Result<IReadOnlyCollection<GroupBasicDetailsScheme>>.Failure(UserErrors.UserDoesNotExist);
    }
}
