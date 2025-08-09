namespace Vinder.IdentityProvider.Application.Providers;

public interface ITenantProvider
{
    public string? Tenant { get; }
    public Task<Result<Tenant>> GetCurrentTenantAsync();
}