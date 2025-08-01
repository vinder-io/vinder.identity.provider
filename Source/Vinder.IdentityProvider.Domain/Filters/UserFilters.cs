namespace Vinder.IdentityProvider.Domain.Filters;

public sealed class UserFilters : PaginationFilters
{
    public Guid? TenantId { get; set; }
    public Guid? UserId { get; set; }

    public string? SecurityToken { get; set; }
    public string? Username { get; set; }
    public bool? IsDeleted { get; set; }
}
