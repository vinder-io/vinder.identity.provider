namespace Vinder.IdentityProvider.Domain.Filters;

public sealed class PermissionFilters : PaginationFilters
{
    public string? TenantId { get; set; }
    public string? PermissionId { get; set; }

    public string? Name { get; set; }
    public bool? IsDeleted { get; set; }
}