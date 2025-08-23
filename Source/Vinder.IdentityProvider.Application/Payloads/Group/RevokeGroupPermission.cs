namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed record RevokeGroupPermission : IRequest<Result>
{
    public Guid PermissionId { get; init; }
    public Guid GroupId { get; init; }
}
