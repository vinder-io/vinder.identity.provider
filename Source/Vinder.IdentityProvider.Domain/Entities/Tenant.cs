namespace Vinder.IdentityProvider.Domain.Entities;

public sealed class Tenant : Entity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public string ClientId { get; set; } = default!;
    public string SecretHash { get; set; } = default!;

    public ICollection<User> Users { get; set; } = [];
    public ICollection<Permission> Permissions { get; set; } = [];
    public ICollection<PermissionGroup> Groups { get; set; } = [];
}
