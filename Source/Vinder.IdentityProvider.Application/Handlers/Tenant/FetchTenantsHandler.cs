namespace Vinder.IdentityProvider.Application.Handlers.Tenant;

public sealed class FetchTenantsHandler(ITenantRepository repository) :
    IRequestHandler<TenantFetchParameters, Result<Pagination<TenantDetails>>>
{
    public async Task<Result<Pagination<TenantDetails>>> Handle(TenantFetchParameters request, CancellationToken cancellationToken)
    {
        var filters = TenantMapper.AsFilters(request);

        var tenants = await repository.GetTenantsAsync(filters, cancellationToken);
        var totalTenants = await repository.CountAsync(filters, cancellationToken);

        var pagination = new Pagination<TenantDetails>
        {
            Items = [.. tenants.Select(tenant => TenantMapper.AsResponse(tenant))],
            Total = (int) totalTenants,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        };

        return Result<Pagination<TenantDetails>>.Success(pagination);
    }
}
