namespace Vinder.IdentityProvider.Application.Payloads.Permission;

public sealed record PermissionForDeletion : IRequest<Result>
{
    public Guid PermissionId { get; init; }
}
