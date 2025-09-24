namespace Vinder.IdentityProvider.Application.Payloads.Permission;

public sealed record PermissionForDeletion : IRequest<Result>
{
    public string PermissionId { get; init; } = default!;
}
