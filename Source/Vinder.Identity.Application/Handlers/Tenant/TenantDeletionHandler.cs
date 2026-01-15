namespace Vinder.Identity.Application.Handlers.Tenant;

public sealed class TenantDeletionHandler(ITenantCollection collection) : IRequestHandler<TenantDeletionScheme, Result>
{
    public async Task<Result> Handle(TenantDeletionScheme request, CancellationToken cancellationToken)
    {
        var filters = new TenantFiltersBuilder()
            .WithIdentifier(request.TenantId)
            .Build();

        var tenants = await collection.GetTenantsAsync(filters, cancellation: cancellationToken);
        var tenant = tenants.FirstOrDefault();

        if (tenant is null)
        {
            return Result.Failure(TenantErrors.TenantDoesNotExist);
        }

        await collection.DeleteAsync(tenant, cancellation: cancellationToken);

        return Result.Success();
    }
}