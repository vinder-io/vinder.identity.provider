namespace Vinder.IdentityProvider.Application.Handlers.Permission;

public sealed class PermissionDeletionHandler(IPermissionRepository repository) : IRequestHandler<PermissionForDeletion, Result>
{
    public async Task<Result> Handle(PermissionForDeletion request, CancellationToken cancellationToken)
    {
        var filters = new PermissionFiltersBuilder()
            .WithPermissionId(request.PermissionId)
            .Build();

        var permissions = await repository.GetPermissionsAsync(filters, cancellation: cancellationToken);
        var permission = permissions.FirstOrDefault();

        if (permission is null)
        {
            return Result.Failure(GroupErrors.GroupDoesNotExist);
        }

        await repository.DeleteAsync(permission, cancellation: cancellationToken);

        return Result.Success();
    }
}