namespace Vinder.IdentityProvider.Application.Payloads.Permission;

public sealed record PermissionDetailsScheme
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string? Description { get; init; } = default!;
}