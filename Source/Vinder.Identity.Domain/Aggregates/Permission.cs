namespace Vinder.Identity.Domain.Aggregates;

public sealed class Permission : Aggregate
{
    public string TenantId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; } = default!;
}
