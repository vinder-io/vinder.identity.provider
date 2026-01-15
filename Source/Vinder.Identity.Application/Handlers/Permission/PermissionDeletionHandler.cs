namespace Vinder.Identity.Application.Handlers.Permission;

public sealed class PermissionDeletionHandler(IPermissionRepository repository) : IRequestHandler<PermissionDeletionScheme, Result>
{
    public async Task<Result> Handle(PermissionDeletionScheme request, CancellationToken cancellationToken)
    {
        var filters = new PermissionFiltersBuilder()
            .WithIdentifier(request.PermissionId)
            .Build();

        var permissions = await repository.GetPermissionsAsync(filters, cancellation: cancellationToken);
        var permission = permissions.FirstOrDefault();

        if (permission is null)
        {
            return Result.Failure(PermissionErrors.PermissionDoesNotExist);
        }

        await repository.DeleteAsync(permission, cancellation: cancellationToken);

        return Result.Success();
    }
}