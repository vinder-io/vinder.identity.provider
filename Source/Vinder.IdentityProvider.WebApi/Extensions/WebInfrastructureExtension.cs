namespace Vinder.IdentityProvider.WebApi.Extensions;

[ExcludeFromCodeCoverage]
public static class WebInfrastructureExtension
{
    public static void AddWebComposition(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddCorsConfiguration();
        services.AddJwtAuthentication();
        services.AddMemoryCache();
        services.AddOpenApi();
    }
}