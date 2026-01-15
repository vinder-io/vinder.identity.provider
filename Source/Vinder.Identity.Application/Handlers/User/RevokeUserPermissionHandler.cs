namespace Vinder.Identity.Application.Handlers.User;

public sealed class RevokeUserPermissionHandler(IUserRepository userRepository, IPermissionRepository permissionRepository) :
    IRequestHandler<RevokeUserPermissionScheme, Result>
{
    public async Task<Result> Handle(RevokeUserPermissionScheme request, CancellationToken cancellationToken)
    {
        var permissionFilters = new PermissionFiltersBuilder()
            .WithIdentifier(request.PermissionId)
            .Build();

        var userFilters = new UserFiltersBuilder()
            .WithIdentifier(request.UserId)
            .Build();

        var users = await userRepository.GetUsersAsync(userFilters, cancellationToken);
        var user = users.FirstOrDefault();

        var permissions = await permissionRepository.GetPermissionsAsync(permissionFilters, cancellationToken);
        var permission = permissions.FirstOrDefault();

        if (user is null)
        {
            return Result.Failure(UserErrors.UserDoesNotExist);
        }

        if (permission is null)
        {
            return Result.Failure(PermissionErrors.PermissionDoesNotExist);
        }

        #pragma warning disable S1764

        // sonar suggests changing the lambda parameter name to avoid confusion,
        // but we prefer keeping it as 'permission' for readability. it does not interfere
        // with the outer variable because C# differentiates the lambda parameter from the argument.

        var permissionToRemove = user.Permissions.FirstOrDefault(permission => permission.Id == permission.Id);
        if (permissionToRemove is null)
        {
            return Result.Failure(UserErrors.PermissionNotAssigned);
        }

        user.Permissions.Remove(permissionToRemove);

        await userRepository.UpdateAsync(user, cancellationToken);

        return Result.Success();
    }
}