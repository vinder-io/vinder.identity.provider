namespace Vinder.Identity.WebApi.Extensions;

public static class OpenApiExtension
{
    public static void AddOpenApiSpecification(this IServiceCollection services)
    {
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
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Enter 'Bearer' and then your valid token."
                };

                document.Components.SecuritySchemes[Headers.Tenant] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = Headers.Tenant,
                    In = ParameterLocation.Header,
                    Description = "Tenant identifier for multi-tenant requests"
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

                document.SecurityRequirements ??= [];
                document.SecurityRequirements.Add(new OpenApiSecurityRequirement
                {
                    [document.Components.SecuritySchemes[SecuritySchemes.Bearer]] = Array.Empty<string>(),
                    [document.Components.SecuritySchemes[Headers.Tenant]] = Array.Empty<string>()
                });

                return Task.CompletedTask;
            });
        });
    }
}