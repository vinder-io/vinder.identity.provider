namespace Vinder.Identity.Application.Handlers.Tenant;

public sealed class TenantUpdateHandler(ITenantCollection collection) :
    IMessageHandler<TenantUpdateScheme, Result<TenantDetailsScheme>>
{
    public async Task<Result<TenantDetailsScheme>> HandleAsync(TenantUpdateScheme parameters, CancellationToken cancellation = default)
    {
        var filters = TenantFilters.WithSpecifications()
            .WithIdentifier(parameters.TenantId)
            .Build();

        var tenants = await collection.GetTenantsAsync(filters, cancellation: cancellation);
        var tenant = tenants.FirstOrDefault();

        if (tenant is null)
        {
            return Result<TenantDetailsScheme>.Failure(TenantErrors.TenantDoesNotExist);
        }

        tenant = TenantMapper.AsTenant(parameters, tenant);

        var updatedTenant = await collection.UpdateAsync(tenant, cancellation: cancellation);

        return Result<TenantDetailsScheme>.Success(TenantMapper.AsResponse(updatedTenant));
    }
}