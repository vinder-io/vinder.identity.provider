namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record ListUserAssignedPermissions :
    IRequest<Result<IReadOnlyCollection<PermissionDetails>>>
{
    public string UserId { get; init; } = default!;
    public string? PermissionName { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
