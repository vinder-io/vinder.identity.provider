namespace Vinder.Identity.Infrastructure.Repositories;

public sealed class SecretRepository(IMongoDatabase database) :
    BaseRepository<Secret>(database, Collections.Secrets),
    ISecretRepository
{
    public async Task<Secret> GetSecretAsync(CancellationToken cancellation = default)
    {
        return await _collection
            .Find(Builders<Secret>.Filter.Empty)
            .FirstOrDefaultAsync(cancellation);
    }
}