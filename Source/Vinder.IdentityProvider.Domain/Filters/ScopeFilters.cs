namespace Vinder.IdentityProvider.Domain.Filters;

public sealed class ScopeFilters : PaginationFilters
{
    public Guid? ScopeId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsDeleted { get; set; }
}
