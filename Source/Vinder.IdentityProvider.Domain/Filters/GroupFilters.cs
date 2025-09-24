namespace Vinder.IdentityProvider.Domain.Filters;

public sealed class GroupFilters : PaginationFilters
{
    public string? Id { get; set; }
    public string? TenantId { get; set; }
    public string? Name { get; set; }
    public bool? IsDeleted { get; set; }
}
