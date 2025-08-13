namespace Vinder.IdentityProvider.Application.Payloads.Permission;

public sealed record PermissionForCreation : IRequest<Result<PermissionDetails>>
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; } = default!;
}