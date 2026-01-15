namespace Vinder.Identity.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage]
public static class ApplicationServicesExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<ISecurityTokenService, JwtSecurityTokenService>();
        services.AddTransient<IHostInformationProvider, HostInformationProvider>();
        services.AddTransient<IClientCredentialsGenerator, ClientCredentialsGenerator>();

        services.AddSingleton<ITenantProvider, TenantProvider>();
        services.AddSingleton<IPrincipalProvider, PrincipalProvider>();
    }
}