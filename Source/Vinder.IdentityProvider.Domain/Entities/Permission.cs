namespace Vinder.IdentityProvider.Domain.Entities;

public sealed class Permission : Entity
{
    public string TenantId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; } = default!;
}
