namespace Vinder.IdentityProvider.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage]
public static class ServicesExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var settings = services.ConfigureSettings(configuration);

        services.AddDataPersistence(settings);
        services.AddServices();
        services.AddMediator();
        services.AddValidators();
        services.AddInitialSecrets();
    }
}