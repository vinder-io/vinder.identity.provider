namespace Vinder.IdentityProvider.Domain.Filters;

public sealed class TokenFilters : PaginationFilters
{
    public TokenType? Type { get; set; }
    public Guid? UserId { get; set; }
    public Guid? TenantId { get; set; }

    public string? Value { get; set; }
    public bool? IsDeleted { get; set; }
}