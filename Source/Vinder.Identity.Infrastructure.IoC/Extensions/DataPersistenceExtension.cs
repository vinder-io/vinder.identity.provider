namespace Vinder.Identity.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage]
public static class DataPersistenceExtension
{
    public static void AddDataPersistence(this IServiceCollection services, ISettings settings)
    {
        services.AddSingleton<IMongoDatabase>(provider =>
        {
            var mongoClient = new MongoClient(settings.Database.ConnectionString);
            var database = mongoClient.GetDatabase(settings.Database.DatabaseName);

            return database;
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IScopeRepository, ScopesRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ISecretRepository, SecretRepository>();
    }
}