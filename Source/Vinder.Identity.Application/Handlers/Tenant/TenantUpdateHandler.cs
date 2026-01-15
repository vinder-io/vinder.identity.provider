namespace Vinder.Identity.Application.Handlers.Tenant;

public sealed class TenantUpdateHandler(ITenantCollection collection) :
    IRequestHandler<TenantUpdateScheme, Result<TenantDetailsScheme>>
{
    public async Task<Result<TenantDetailsScheme>> Handle(TenantUpdateScheme request, CancellationToken cancellationToken)
    {
        var filters = new TenantFiltersBuilder()
            .WithIdentifier(request.TenantId)
            .Build();

        var tenants = await collection.GetTenantsAsync(filters, cancellation: cancellationToken);
        var tenant = tenants.FirstOrDefault();

        if (tenant is null)
        {
            return Result<TenantDetailsScheme>.Failure(TenantErrors.TenantDoesNotExist);
        }

        tenant = TenantMapper.AsTenant(request, tenant);

        var updatedTenant = await collection.UpdateAsync(tenant, cancellation: cancellationToken);

        return Result<TenantDetailsScheme>.Success(TenantMapper.AsResponse(updatedTenant));
    }
}