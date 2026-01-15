namespace Vinder.Identity.Application.Payloads.User;

public sealed record ListUserAssignedPermissionsParameters :
    IRequest<Result<IReadOnlyCollection<PermissionDetailsScheme>>>
{
    public string UserId { get; init; } = default!;
    public string? PermissionName { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
