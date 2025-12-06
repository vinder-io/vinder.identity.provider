namespace Vinder.IdentityProvider.Application.Handlers.User;

public sealed class ListUserAssignedGroupsHandler(IUserRepository repository) :
    IRequestHandler<ListUserAssignedGroupsParameters, Result<IReadOnlyCollection<GroupBasicDetailsScheme>>>
{
    public async Task<Result<IReadOnlyCollection<GroupBasicDetailsScheme>>> Handle(
        ListUserAssignedGroupsParameters request, CancellationToken cancellationToken)
    {
        var filters = new UserFiltersBuilder()
            .WithIdentifier(request.UserId)
            .Build();

        var users = await repository.GetUsersAsync(filters, cancellationToken);
        var user = users.FirstOrDefault();

        return user is not null
            ? Result<IReadOnlyCollection<GroupBasicDetailsScheme>>.Success(GroupMapper.AsBasicResponse(user.Groups))
            : Result<IReadOnlyCollection<GroupBasicDetailsScheme>>.Failure(UserErrors.UserDoesNotExist);
    }
}
