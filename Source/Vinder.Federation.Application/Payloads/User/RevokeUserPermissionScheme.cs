namespace Vinder.Federation.Application.Payloads.User;

public sealed record RevokeUserPermissionScheme : IMessage<Result>
{
    public string UserId { get; init; } = default!;
    public string PermissionId { get; init; } = default!;
}
