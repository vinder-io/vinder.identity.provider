namespace Vinder.IdentityProvider.Application.Providers;

public interface ITenantProvider
{
    public string? Tenant { get; }

    public void SetTenant(Tenant tenant);
    public Tenant GetCurrentTenant();
}