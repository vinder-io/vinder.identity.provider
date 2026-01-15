namespace Vinder.Identity.Application.Handlers.User;

public sealed class ListUserAssignedPermissionsHandler(IUserCollection collection) :
    IRequestHandler<ListUserAssignedPermissionsParameters, Result<IReadOnlyCollection<PermissionDetailsScheme>>>
{
    public async Task<Result<IReadOnlyCollection<PermissionDetailsScheme>>> Handle(
        ListUserAssignedPermissionsParameters request, CancellationToken cancellationToken)
    {
        var filters = new UserFiltersBuilder()
            .WithIdentifier(request.UserId)
            .Build();

        var users = await collection.GetUsersAsync(filters, cancellationToken);
        var user = users.FirstOrDefault();

        return user is not null
            ? Result<IReadOnlyCollection<PermissionDetailsScheme>>.Success(PermissionMapper.AsResponse(user.Permissions))
            : Result<IReadOnlyCollection<PermissionDetailsScheme>>.Failure(UserErrors.UserDoesNotExist);
    }
}
