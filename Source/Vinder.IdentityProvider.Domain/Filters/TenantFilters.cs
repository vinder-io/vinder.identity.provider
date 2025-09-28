namespace Vinder.IdentityProvider.Domain.Filters;

public sealed class TenantFilters : PaginationFilters
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? ClientId { get; set; }
    public bool? IsDeleted { get; set; }
}