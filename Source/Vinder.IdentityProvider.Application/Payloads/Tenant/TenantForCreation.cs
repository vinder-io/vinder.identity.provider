namespace Vinder.IdentityProvider.Application.Payloads.Tenant;

public sealed record TenantForCreation : IRequest<Result<TenantDetails>>
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; } = default!;
}