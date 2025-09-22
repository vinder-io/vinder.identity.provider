namespace Vinder.IdentityProvider.Domain.Entities;

public sealed class Scope : Entity
{
    public Guid TenantId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsGlobal { get; set; }
}