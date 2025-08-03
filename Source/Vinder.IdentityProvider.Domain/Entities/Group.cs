namespace Vinder.IdentityProvider.Domain.Entities;

public sealed class Group : Entity
{
    public Guid TenantId { get; set; }

    public string Name { get; set; } = default!;
    public ICollection<Permission> Permissions { get; set; } = [  ];
}
