namespace Vinder.IdentityProvider.Application.Payloads.Tenant;

public sealed record TenantForDeletion : IRequest<Result>
{
    public Guid TenantId { get; init; }
}