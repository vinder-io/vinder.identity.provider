namespace Vinder.IdentityProvider.Domain.Entities;

public sealed class User : Entity
{
    public Guid TenantId { get; set; }

    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;

    public ICollection<Permission> Permissions { get; set; } = [];
    public ICollection<PermissionGroup> Groups { get; set; } = [];
    public ICollection<SecurityToken> Tokens { get; set; } = [];
}