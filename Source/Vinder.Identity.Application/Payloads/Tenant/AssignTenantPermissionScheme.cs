namespace Vinder.Identity.Application.Payloads.Tenant;

public sealed record AssignTenantPermissionScheme : IMessage<Result<IReadOnlyCollection<PermissionDetailsScheme>>>
{
    [JsonIgnore]
    public string TenantId { get; init; } = default!;
    public string PermissionName { get; init; } = default!;
}
