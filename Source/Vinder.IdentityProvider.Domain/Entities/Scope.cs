namespace Vinder.IdentityProvider.Domain.Entities;

public sealed class Scope : Entity
{
    public string TenantId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsGlobal { get; set; }
}