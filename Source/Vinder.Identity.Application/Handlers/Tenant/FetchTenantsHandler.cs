namespace Vinder.Identity.Application.Handlers.Tenant;

public sealed class FetchTenantsHandler(ITenantRepository repository) :
    IRequestHandler<TenantFetchParameters, Result<Pagination<TenantDetailsScheme>>>
{
    public async Task<Result<Pagination<TenantDetailsScheme>>> Handle(
        TenantFetchParameters parameters, CancellationToken cancellationToken)
    {
        var filters = TenantMapper.AsFilters(parameters);

        var tenants = await repository.GetTenantsAsync(filters, cancellationToken);
        var totalTenants = await repository.CountAsync(filters, cancellationToken);

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
