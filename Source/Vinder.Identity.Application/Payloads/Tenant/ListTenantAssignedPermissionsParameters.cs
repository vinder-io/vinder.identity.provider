namespace Vinder.Identity.Application.Payloads.Tenant;

public sealed record ListTenantAssignedPermissionsParameters :
    IMessage<Result<IReadOnlyCollection<PermissionDetailsScheme>>>
{
    public string? TenantId { get; init; } = default!;
    public string? PermissionName { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
