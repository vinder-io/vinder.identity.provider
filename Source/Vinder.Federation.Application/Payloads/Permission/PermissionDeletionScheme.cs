namespace Vinder.Federation.Application.Payloads.Permission;

public sealed record PermissionDeletionScheme : IMessage<Result>
{
    public string PermissionId { get; init; } = default!;
}
