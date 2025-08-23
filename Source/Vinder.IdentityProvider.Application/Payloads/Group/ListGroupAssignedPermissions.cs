namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed record ListGroupAssignedPermissions :
    IRequest<Result<IReadOnlyCollection<PermissionDetails>>>
{
    public Guid GroupId { get; init; }
    public string? PermissionName { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
