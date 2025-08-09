namespace Vinder.IdentityProvider.WebApi.Middlewares;

[ExcludeFromCodeCoverage]
public static class TenantMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TenantMiddleware>();
    }
}
