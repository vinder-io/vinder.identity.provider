namespace Vinder.IdentityProvider.Domain.Filters;

public sealed class PermissionFilters : PaginationFilters
{
    public Guid? TenantId { get; set; }
    public Guid? PermissionId { get; set; }

    public string? Name { get; set; }
    public bool? IsDeleted { get; set; }
}