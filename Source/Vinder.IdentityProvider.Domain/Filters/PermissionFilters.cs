namespace Vinder.IdentityProvider.Domain.Filters;

public sealed class PermissionFilters : PaginationFilters
{
    public string? Name { get; set; }
    public Guid? TenantId { get; set; }
    public bool? IsDeleted { get; set; }
}