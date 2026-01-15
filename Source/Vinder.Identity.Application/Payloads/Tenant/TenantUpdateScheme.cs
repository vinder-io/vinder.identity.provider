namespace Vinder.Identity.Application.Payloads.Tenant;

public sealed record TenantUpdateScheme : IRequest<Result<TenantDetailsScheme>>
{
    [JsonIgnore]
    public string TenantId { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string? Description { get; init; } = default!;
}