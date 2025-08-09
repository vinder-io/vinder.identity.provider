namespace Vinder.IdentityProvider.WebApi.Middlewares;

public sealed class TenantMiddleware(RequestDelegate next)
{
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

        context.Items["TenantName"] = context.Request.Headers[tenantHeaderKey].ToString();

        await next(context);
    }
}
