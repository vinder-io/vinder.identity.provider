namespace Vinder.IdentityProvider.Application.Payloads.Tenant;

public sealed record TenantFetchParameters :
    IRequest<Result<Pagination<TenantDetailsScheme>>>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? ClientId { get; init; }
    public bool? IsDeleted { get; init; }

    public PaginationFilters? Pagination { get; set; }
    public SortFilters? Sort { get; set; }

    public DateOnly? CreatedAfter { get; set; }
    public DateOnly? CreatedBefore { get; set; }
}