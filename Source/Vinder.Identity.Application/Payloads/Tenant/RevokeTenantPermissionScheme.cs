namespace Vinder.Identity.Application.Payloads.Tenant;

public sealed record RevokeTenantPermissionScheme : IMessage<Result>
{
    [JsonIgnore]
    public string TenantId { get; init; } = default!;
    public string PermissionId { get; init; } = default!;
}
