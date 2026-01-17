namespace Vinder.Identity.Application.Handlers.User;

public sealed class ListUserAssignedPermissionsHandler(IUserCollection collection) :
    IMessageHandler<ListUserAssignedPermissionsParameters, Result<IReadOnlyCollection<PermissionDetailsScheme>>>
{
    public async Task<Result<IReadOnlyCollection<PermissionDetailsScheme>>> HandleAsync(
        ListUserAssignedPermissionsParameters parameters, CancellationToken cancellation = default)
    {
        var filters = new UserFiltersBuilder()
            .WithIdentifier(parameters.UserId)
            .Build();

        var users = await collection.GetUsersAsync(filters, cancellation);
        var user = users.FirstOrDefault();

        return user is not null
            ? Result<IReadOnlyCollection<PermissionDetailsScheme>>.Success(PermissionMapper.AsResponse(user.Permissions))
            : Result<IReadOnlyCollection<PermissionDetailsScheme>>.Failure(UserErrors.UserDoesNotExist);
    }
}
