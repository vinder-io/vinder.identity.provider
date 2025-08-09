namespace Vinder.IdentityProvider.Infrastructure.Providers;

public sealed class TenantProvider : ITenantProvider
{
    private Tenant? _currentTenant;
    public string? Tenant => _currentTenant?.Name;

    #pragma warning disable S1121 // sonarqube(csharpsquid:S1121)
    public async Task SetTenantAsync(Tenant tenant, CancellationToken cancellation = default) =>
        await Task.FromResult(_currentTenant = tenant);

    public async Task<Tenant> GetCurrentTenantAsync(CancellationToken cancellation = default) =>
        await Task.FromResult(_currentTenant!);
}
