namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record RevokeUserPermission : IRequest<Result>
{
    public string UserId { get; init; } = default!;
    public string PermissionId { get; init; } = default!;
}
