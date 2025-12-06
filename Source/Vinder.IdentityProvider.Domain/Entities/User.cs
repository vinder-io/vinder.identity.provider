namespace Vinder.IdentityProvider.Domain.Entities;

public sealed class User : Entity
{
    public string TenantId { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;

    public ICollection<Permission> Permissions { get; set; } = [];
    public ICollection<Group> Groups { get; set; } = [];
}