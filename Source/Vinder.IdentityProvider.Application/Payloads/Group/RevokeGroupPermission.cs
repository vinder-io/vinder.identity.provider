namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed record RevokeGroupPermission : IRequest<Result>
{
    public string PermissionId { get; init; } = default!;
    public string GroupId { get; init; } = default!;
}
