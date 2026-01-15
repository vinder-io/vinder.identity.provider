namespace Vinder.Identity.WebApi.Extensions;

[ExcludeFromCodeCoverage]
public static class WebInfrastructureExtension
{
    public static void AddWebComposition(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddCorsConfiguration();
        services.AddJwtAuthentication();
        services.AddMemoryCache();

        var provider = services.BuildServiceProvider();
        var host = provider.GetRequiredService<IHostInformationProvider>();

        services.AddOpenApi(options =>
        {
            options.AddScalarTransformers();
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();
                document.Components.SecuritySchemes[SecuritySchemes.Bearer] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Enter 'Bearer' and then your valid token."
                };

                document.Components.SecuritySchemes[SecuritySchemes.OAuth2] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri(host.Address.ToString() + "api/v1/openid/connect/token")
                        }
                    }
                };

                return Task.CompletedTask;
            });
        });
    }
}