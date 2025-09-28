namespace Vinder.IdentityProvider.Application.Payloads.Scope;

public sealed record ScopeDetails
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
}