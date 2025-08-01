namespace Vinder.IdentityProvider.Domain.Entities;

public sealed class PermissionGroup : Entity
{
    public Guid TenantId { get; set; }

    public string Name { get; set; } = default!;
    public ICollection<Permission> Permissions { get; set; } = [  ];
}
