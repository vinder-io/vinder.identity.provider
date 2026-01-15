namespace Vinder.Identity.Domain.Entities;

public sealed class Group : Entity
{
    public string TenantId { get; set; } = default!;
    public string Name { get; set; } = default!;

    public ICollection<Permission> Permissions { get; set; } = [];
}
