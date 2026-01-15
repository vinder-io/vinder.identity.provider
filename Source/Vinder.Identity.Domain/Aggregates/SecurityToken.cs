namespace Vinder.Identity.Domain.Aggregates;

public sealed class SecurityToken : Entity
{
    public string Value { get; set; } = default!;
    public bool Revoked { get; set; }

    public TokenType Type { get; set; }
    public DateTime ExpiresAt { get; set; }

    public string UserId { get; set; } = default!;
    public string TenantId { get; set; } = default!;

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !Revoked && !IsExpired;
}