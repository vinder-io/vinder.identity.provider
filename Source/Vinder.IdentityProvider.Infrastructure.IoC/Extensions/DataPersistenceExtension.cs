namespace Vinder.IdentityProvider.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage]
public static class DataPersistenceExtension
{
    public static void AddDataPersistence(this IServiceCollection services, ISettings settings)
    {
        /* register custom serializers globally so mongodb can correctly serialize/deserialize custom types */
        MongoSerializer.Register();

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
    }
}