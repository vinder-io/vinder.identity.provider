namespace Vinder.IdentityProvider.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage]
public static class ApplicationServicesExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ISecurityTokenService, JwtSecurityTokenService>();
        services.AddScoped<ITenantProvider, TenantProvider>();
        services.AddScoped<IPrincipalProvider, PrincipalProvider>();
        services.AddScoped<IHostInformationProvider, HostInformationProvider>();
        services.AddScoped<IClientCredentialsGenerator, ClientCredentialsGenerator>();
    }
}