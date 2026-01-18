namespace Vinder.Federation.Application.Handlers.Tenant;

public sealed class TenantDeletionHandler(ITenantCollection collection) : IMessageHandler<TenantDeletionScheme, Result>
{
    public async Task<Result> HandleAsync(TenantDeletionScheme parameters, CancellationToken cancellation = default)
    {
        var filters = TenantFilters.WithSpecifications()
            .WithIdentifier(parameters.TenantId)
            .Build();

        var tenants = await collection.GetTenantsAsync(filters, cancellation: cancellation);
        var tenant = tenants.FirstOrDefault();

        if (tenant is null)
        {
            return Result.Failure(TenantErrors.TenantDoesNotExist);
        }

        await collection.DeleteAsync(tenant, cancellation: cancellation);

        return Result.Success();
    }
}