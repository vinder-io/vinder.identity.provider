namespace Vinder.IdentityProvider.Application.Payloads.Scope;

public sealed record ScopeForCreation : IRequest<Result<ScopeDetails>>
{
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;

    [JsonIgnore]
    public bool IsGlobal { get; init; }
}