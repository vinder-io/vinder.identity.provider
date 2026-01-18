namespace Vinder.Federation.Domain.Aggregates;

public sealed class Scope : Aggregate
{
    public string TenantId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsGlobal { get; set; }
}