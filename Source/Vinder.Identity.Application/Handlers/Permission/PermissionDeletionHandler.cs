namespace Vinder.Identity.Application.Handlers.Permission;

public sealed class PermissionDeletionHandler(IPermissionCollection collection) : IRequestHandler<PermissionDeletionScheme, Result>
{
    public async Task<Result> Handle(PermissionDeletionScheme request, CancellationToken cancellationToken)
    {
        var filters = new PermissionFiltersBuilder()
            .WithIdentifier(request.PermissionId)
            .Build();

        var permissions = await collection.GetPermissionsAsync(filters, cancellation: cancellationToken);
        var permission = permissions.FirstOrDefault();

        if (permission is null)
        {
            return Result.Failure(PermissionErrors.PermissionDoesNotExist);
        }

        await collection.DeleteAsync(permission, cancellation: cancellationToken);

        return Result.Success();
    }
}