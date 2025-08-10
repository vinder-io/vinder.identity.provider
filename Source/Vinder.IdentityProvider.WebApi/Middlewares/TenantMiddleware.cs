namespace Vinder.IdentityProvider.WebApi.Middlewares;

public sealed class TenantMiddleware(IMemoryCache cache, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var requiresTenant = endpoint?.Metadata.GetMetadata<TenantRequiredAttribute>() != null;

        if (!requiresTenant)
        {
            await next(context);
            return;
        }

        var tenantRepository = context.RequestServices.GetRequiredService<ITenantRepository>();
        var tenantProvider = context.RequestServices.GetRequiredService<ITenantProvider>();

        var tenantHeaderKey = context.Request.Headers.Keys
            .FirstOrDefault(key => string.Equals(key, "x-tenant", StringComparison.OrdinalIgnoreCase));

        if (tenantHeaderKey == null || string.IsNullOrWhiteSpace(context.Request.Headers[tenantHeaderKey]))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            var error = TenantErrors.TenantHeaderMissing;
            var response = JsonSerializer.Serialize(new
            {
                code = error.Code,
                description = error.Description
            });

            await context.Response.WriteAsync(response);
            return;
        }

        var tenantName = context.Request.Headers[tenantHeaderKey].ToString();
        var cacheKey = $"tenant:{tenantName}";

        if (!cache.TryGetValue(cacheKey, out Tenant? tenant))
        {
            var filters = new TenantFiltersBuilder()
                .WithName(tenantName)
                .Build();

            var tenants = await tenantRepository.GetTenantsAsync(filters, context.RequestAborted);

            tenant = tenants.FirstOrDefault();

            if (tenant is null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = MediaTypeNames.Application.Json;

                var error = TenantErrors.TenantDoesNotExist;
                var response = JsonSerializer.Serialize(new
                {
                    code = error.Code,
                    description = error.Description
                });

                await context.Response.WriteAsync(response);
                return;
            }

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromDays(10));

            cache.Set(cacheKey, tenant, cacheOptions);
        }

        context.Items["TenantName"] = tenant?.Name;
        tenantProvider.SetTenant(tenant!);

        await next(context);
    }
}
