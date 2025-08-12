namespace Vinder.IdentityProvider.Application.Payloads.Permission;

public sealed record PermissionDetails
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
}