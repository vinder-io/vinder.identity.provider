namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record RevokeUserPermission : IRequest<Result>
{
    public Guid UserId { get; init; }
    public Guid PermissionId { get; init; }
}
