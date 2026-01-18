namespace Vinder.Federation.Infrastructure.Providers;

public sealed class TenantProvider : ITenantProvider
{
    private Tenant? _currentTenant;
    public string? Tenant => _currentTenant?.Name;

    public void SetTenant(Tenant tenant) =>
        _currentTenant = tenant;

    public Tenant GetCurrentTenant() =>
        _currentTenant!;
}
