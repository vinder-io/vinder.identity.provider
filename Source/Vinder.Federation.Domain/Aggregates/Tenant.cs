namespace Vinder.Federation.Domain.Aggregates;

public sealed class Tenant : Aggregate
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public string ClientId { get; set; } = default!;
    public string SecretHash { get; set; } = default!;

    public ICollection<Permission> Permissions { get; set; } = [];
    public ICollection<Scope> Scopes { get; set; } = [];
}
