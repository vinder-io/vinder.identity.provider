namespace Vinder.Federation.Application.Payloads.Group;

public sealed record RevokeGroupPermissionScheme : IMessage<Result>
{
    public string PermissionId { get; init; } = default!;
    public string GroupId { get; init; } = default!;
}
