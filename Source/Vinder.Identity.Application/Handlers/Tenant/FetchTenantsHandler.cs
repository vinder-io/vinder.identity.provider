namespace Vinder.Identity.Application.Handlers.Tenant;

public sealed class FetchTenantsHandler(ITenantCollection collection) :
    IMessageHandler<TenantFetchParameters, Result<Pagination<TenantDetailsScheme>>>
{
    public async Task<Result<Pagination<TenantDetailsScheme>>> HandleAsync(
        TenantFetchParameters parameters, CancellationToken cancellation)
    {
        var filters = TenantMapper.AsFilters(parameters);

        var tenants = await collection.GetTenantsAsync(filters, cancellation);
        var totalTenants = await collection.CountAsync(filters, cancellation);

        var pagination = new Pagination<TenantDetailsScheme>
        {
            Items = [.. tenants.Select(tenant => TenantMapper.AsResponse(tenant))],
            Total = (int) totalTenants,
            PageNumber = parameters.Pagination?.PageNumber ?? 1,
            PageSize = parameters.Pagination?.PageSize ?? 20
        };

        return Result<Pagination<TenantDetailsScheme>>.Success(pagination);
    }
}
