namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed record GroupDetails
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
}