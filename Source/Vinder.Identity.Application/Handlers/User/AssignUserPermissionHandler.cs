namespace Vinder.Identity.Application.Handlers.User;

public sealed class AssignUserPermissionHandler(IUserCollection userCollection, IPermissionCollection permissionCollection) :
    IRequestHandler<AssignUserPermissionScheme, Result>
{
    public async Task<Result> Handle(AssignUserPermissionScheme request, CancellationToken cancellationToken)
    {
        var userFilters = new UserFiltersBuilder()
            .WithIdentifier(request.UserId)
            .Build();

        var permissionFilters = new PermissionFiltersBuilder()
            .WithName(request.PermissionName.ToLower())
            .Build();

        var users = await userCollection.GetUsersAsync(userFilters, cancellation: cancellationToken);
        var user = users.FirstOrDefault();

        if (user is null)
        {
            return Result.Failure(UserErrors.UserDoesNotExist);
        }

        var permissions = await permissionCollection.GetPermissionsAsync(permissionFilters, cancellation: cancellationToken);
        var existingPermission = permissions.FirstOrDefault();

        if (existingPermission is null)
        {
            return Result.Failure(PermissionErrors.PermissionDoesNotExist);
        }

        if (user.Permissions.Any(permission => permission.Name == existingPermission.Name))
        {
            return Result.Failure(UserErrors.UserAlreadyHasPermission);
        }

        user.Permissions.Add(existingPermission);

        await userCollection.UpdateAsync(user, cancellation: cancellationToken);

        return Result.Success();
    }
}