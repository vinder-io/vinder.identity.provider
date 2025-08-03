namespace Vinder.IdentityProvider.Domain.Filters;

public sealed class GroupFilters : PaginationFilters
{
    public Guid? Id { get; set; }
    public Guid? TenantId { get; set; }
    public string? Name { get; set; }
    public bool? IsDeleted { get; set; }
}
