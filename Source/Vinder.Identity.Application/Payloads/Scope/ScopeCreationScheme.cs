namespace Vinder.Identity.Application.Payloads.Scope;

public sealed record ScopeCreationScheme : IRequest<Result<ScopeDetailsScheme>>
{
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;

    [JsonIgnore]
    public bool IsGlobal { get; init; }
}