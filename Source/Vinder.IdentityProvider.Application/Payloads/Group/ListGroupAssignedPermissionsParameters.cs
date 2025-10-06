namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed record ListGroupAssignedPermissionsParameters :
    IRequest<Result<IReadOnlyCollection<PermissionDetailsScheme>>>
{
    public string GroupId { get; init; } = default!;
    public string? PermissionName { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
