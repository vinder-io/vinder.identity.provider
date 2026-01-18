namespace Vinder.Federation.Application.Payloads.Permission;

public sealed record PermissionCreationScheme : IMessage<Result<PermissionDetailsScheme>>
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; } = default!;
}