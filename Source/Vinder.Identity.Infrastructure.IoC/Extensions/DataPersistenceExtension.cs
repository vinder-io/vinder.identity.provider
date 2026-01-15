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

        services.AddScoped<IUserCollection, UserCollection>();
        services.AddScoped<IPermissionCollection, PermissionCollection>();
        services.AddScoped<IGroupCollection, GroupCollection>();
        services.AddScoped<IScopeCollection, ScopesCollection>();
        services.AddScoped<ITokenCollection, TokenCollection>();
        services.AddScoped<ITenantCollection, TenantCollection>();
        services.AddScoped<ISecretCollection, SecretCollection>();
    }
}