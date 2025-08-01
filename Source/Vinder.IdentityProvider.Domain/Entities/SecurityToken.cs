namespace Vinder.IdentityProvider.Domain.Entities;

public sealed class SecurityToken : Entity
{
    public string Value { get; set; } = default!;
    public bool Revoked { get; set; }

    public TokenType Type { get; set; }
    public DateTime ExpiresAt { get; set; }

    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !Revoked && !IsExpired;
}