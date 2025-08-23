namespace Vinder.IdentityProvider.Application.Payloads.Tenant;

public sealed record TenantForUpdate : IRequest<Result<TenantDetails>>
{
    [JsonIgnore]
    public Guid TenantId { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; } = default!;
}