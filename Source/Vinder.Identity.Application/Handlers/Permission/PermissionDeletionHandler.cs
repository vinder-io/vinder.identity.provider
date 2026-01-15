namespace Vinder.Identity.Application.Handlers.Permission;

public sealed class PermissionDeletionHandler(IPermissionCollection collection) : IMessageHandler<PermissionDeletionScheme, Result>
{
    public async Task<Result> HandleAsync(PermissionDeletionScheme parameters, CancellationToken cancellation)
    {
        var filters = new PermissionFiltersBuilder()
            .WithIdentifier(parameters.PermissionId)
            .Build();

        var permissions = await collection.GetPermissionsAsync(filters, cancellation: cancellation);
        var permission = permissions.FirstOrDefault();

        if (permission is null)
        {
            return Result.Failure(PermissionErrors.PermissionDoesNotExist);
        }

        await collection.DeleteAsync(permission, cancellation: cancellation);

        return Result.Success();
    }
}