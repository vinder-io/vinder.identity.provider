namespace Vinder.IdentityProvider.TestSuite.IntegrationTests.Fixtures;

public sealed class MongoDatabaseFixture : IAsyncLifetime
{
    public string ConnectionString { get; private set; } = string.Empty;
    public string DatabaseName { get; private set; } = "vinder-identity-integration-tests";

    private readonly IContainer _container;

    public IMongoDatabase Database { get; private set; } = default!;
    public IMongoClient Client { get; private set; } = default!;

    public MongoDatabaseFixture()
    {
        _container = new ContainerBuilder()
            .WithImage("mongo:latest")
            .WithCleanUp(true)
            .WithExposedPort(27017)
            .WithPortBinding(0, 27017)
            .WithEnvironment("MONGO_INITDB_ROOT_USERNAME", "admin")
            .WithEnvironment("MONGO_INITDB_ROOT_PASSWORD", "admin")
            .Build();
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var hostPort = _container.GetMappedPublicPort(27017);

        ConnectionString = $"mongodb://admin:admin@localhost:{hostPort}/{DatabaseName}?authSource=admin";

        Client = new MongoClient(ConnectionString);
        Database = Client.GetDatabase(DatabaseName);
    }

    public async Task CleanDatabaseAsync()
    {
        var collections = await Database
            .ListCollectionNames()
            .ToListAsync();

        foreach (var collectionName in collections)
        {
            await Database.DropCollectionAsync(collectionName);
        }
    }
}