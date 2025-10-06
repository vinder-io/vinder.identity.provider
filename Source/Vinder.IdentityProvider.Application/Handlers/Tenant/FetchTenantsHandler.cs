namespace Vinder.IdentityProvider.Application.Handlers.Tenant;

public sealed class FetchTenantsHandler(ITenantRepository repository) :
    IRequestHandler<TenantFetchParameters, Result<Pagination<TenantDetailsScheme>>>
{
    public async Task<Result<Pagination<TenantDetailsScheme>>> Handle(TenantFetchParameters request, CancellationToken cancellationToken)
    {
        var filters = TenantMapper.AsFilters(request);

        var tenants = await repository.GetTenantsAsync(filters, cancellationToken);
        var totalTenants = await repository.CountAsync(filters, cancellationToken);

        var pagination = new Pagination<TenantDetailsScheme>
        {
            Items = [.. tenants.Select(tenant => TenantMapper.AsResponse(tenant))],
            Total = (int) totalTenants,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        };

        return Result<Pagination<TenantDetailsScheme>>.Success(pagination);
    }
}
