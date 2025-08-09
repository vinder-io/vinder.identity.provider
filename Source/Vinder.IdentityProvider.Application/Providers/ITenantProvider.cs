namespace Vinder.IdentityProvider.Application.Providers;

public interface ITenantProvider
{
    public string? Tenant { get; }

    public Task SetTenantAsync(Tenant tenant, CancellationToken cancellation = default);
    public Task<Tenant> GetCurrentTenantAsync(CancellationToken cancellation = default);
}