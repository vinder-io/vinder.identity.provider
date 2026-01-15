namespace Vinder.Identity.Domain.Aggregates;

public sealed class Secret : Aggregate
{
    public string PrivateKey { get; set; } = default!;
    public string PublicKey { get; set; } = default!;
}