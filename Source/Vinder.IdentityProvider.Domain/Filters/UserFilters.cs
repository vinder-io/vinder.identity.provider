namespace Vinder.IdentityProvider.Domain.Filters;

public sealed class UserFilters : PaginationFilters
{
    public string? TenantId { get; set; }
    public string? UserId { get; set; }
    public string? SecurityToken { get; set; }
    public string? Username { get; set; }
    public bool? IsDeleted { get; set; }
}
