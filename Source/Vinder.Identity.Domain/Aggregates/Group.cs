namespace Vinder.Identity.Domain.Aggregates;

public sealed class Group : Entity
{
    public string TenantId { get; set; } = default!;
    public string Name { get; set; } = default!;

    public ICollection<Permission> Permissions { get; set; } = [];
}
