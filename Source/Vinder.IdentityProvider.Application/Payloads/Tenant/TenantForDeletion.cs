namespace Vinder.IdentityProvider.Application.Payloads.Tenant;

public sealed record TenantForDeletion : IRequest<Result>
{
    public string TenantId { get; init; } = default!;
}