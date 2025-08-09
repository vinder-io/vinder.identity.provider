namespace Vinder.IdentityProvider.WebApi.Middlewares;

public sealed class TenantMiddleware
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantProvider _tenantProvider;
    private readonly IMemoryCache _memoryCache;
    private readonly RequestDelegate _next;

    public TenantMiddleware(ITenantRepository repository, ITenantProvider provider, IMemoryCache memoryCache, RequestDelegate next)
    {
        _tenantRepository = repository;
        _tenantProvider = provider;
        _memoryCache = memoryCache;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var tenantHeaderKey = context.Request.Headers.Keys
            .FirstOrDefault(key => string.Equals(key, "X-Tenant", StringComparison.OrdinalIgnoreCase));

        if (tenantHeaderKey == null || string.IsNullOrWhiteSpace(context.Request.Headers[tenantHeaderKey]))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            var error = TenantErrors.TenantHeaderMissing;
            var response = JsonSerializer.Serialize(new
            {
                error = new
                {
                    code = error.Code,
                    description = error.Description
                }
            });

            await context.Response.WriteAsync(response);
            return;
        }

        var tenantName = context.Request.Headers[tenantHeaderKey].ToString();
        var cacheKey = $"tenant:{tenantName}";

        if (!_memoryCache.TryGetValue(cacheKey, out Tenant? tenant))
        {
            var filters = new TenantFiltersBuilder()
                .WithName(tenantName)
                .Build();

            var tenants = await _tenantRepository.GetTenantsAsync(filters, context.RequestAborted);

            tenant = tenants.FirstOrDefault();

            if (tenant is null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = MediaTypeNames.Application.Json;

                var error = TenantErrors.TenantDoesNotExist;
                var response = JsonSerializer.Serialize(new
                {
                    error = new
                    {
                        code = error.Code,
                        description = error.Description
                    }
                });

                await context.Response.WriteAsync(response);
                return;
            }

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromDays(10));

            _memoryCache.Set(cacheKey, tenant, cacheOptions);
        }

        context.Items["TenantName"] = tenant?.Name;

        await _tenantProvider.SetTenantAsync(tenant!);
        await _next(context);
    }
}
