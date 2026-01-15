namespace Vinder.Identity.Application.Payloads.Permission;

public sealed record PermissionCreationScheme : IRequest<Result<PermissionDetailsScheme>>
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; } = default!;
}