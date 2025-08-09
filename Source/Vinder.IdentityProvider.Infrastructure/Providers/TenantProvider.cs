using Microsoft.AspNetCore.Http;

namespace Vinder.IdentityProvider.Infrastructure.Providers;

public sealed class TenantProvider(IHttpContextAccessor contextAccessor) : ITenantProvider
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
    private Tenant? _currentTenant;
    public string? Tenant => _currentTenant?.Name;

    public async Task<Result<Tenant>> GetCurrentTenantAsync()
    {
        if (_currentTenant is not null)
        {
            return await Task.FromResult(Result<Tenant>.Success(_currentTenant));
        }

        var httpContext = _contextAccessor.HttpContext;
        if (httpContext is null)
        {
            return await Task.FromResult(Result<Tenant>.Failure(TenantErrors.HttpContextUnavailable));
        }

        var tenantName = httpContext.Items["TenantName"] as string;
        if (string.IsNullOrEmpty(tenantName))
        {
            return await Task.FromResult(Result<Tenant>.Failure(TenantErrors.TenantHeaderMissing));
        }

        _currentTenant = new Tenant
        {
            Name = tenantName,
        };

        return await Task.FromResult(Result<Tenant>.Success(_currentTenant));
    }
}
