namespace Vinder.IdentityProvider.Domain.Filters;

public sealed class TokenFilters : PaginationFilters
{
    public string? Value { get; set; }
    public TokenType? Type { get; set; }

    public Guid? UserId { get; set; }
    public Guid? TenantId { get; set; }
}