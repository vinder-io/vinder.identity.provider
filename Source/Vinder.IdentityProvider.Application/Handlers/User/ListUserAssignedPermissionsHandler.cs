namespace Vinder.IdentityProvider.Application.Handlers.User;

public sealed class ListUserAssignedPermissionsHandler(IUserRepository repository) :
    IRequestHandler<ListUserAssignedPermissions, Result<IReadOnlyCollection<PermissionDetails>>>
{
    public async Task<Result<IReadOnlyCollection<PermissionDetails>>> Handle(ListUserAssignedPermissions request, CancellationToken cancellationToken)
    {
        var filters = new UserFiltersBuilder()
            .WithUserId(request.UserId)
            .Build();

        var users = await repository.GetUsersAsync(filters, cancellationToken);
        var user = users.FirstOrDefault();

        return user is not null
            ? Result<IReadOnlyCollection<PermissionDetails>>.Success(PermissionMapper.AsResponse(user.Permissions))
            : Result<IReadOnlyCollection<PermissionDetails>>.Failure(UserErrors.UserDoesNotExist);
    }
}
