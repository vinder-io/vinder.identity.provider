namespace Vinder.IdentityProvider.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity>(IMongoDatabase database, string collection) :
    IRepository<TEntity> where TEntity : Entity
{
    protected readonly IMongoCollection<TEntity> _collection = database.GetCollection<TEntity>(collection);

    public virtual async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellation = default)
    {
        entity.MarkAsDeleted();
        entity.MarkAsUpdated();

        var filter = Builders<TEntity>.Filter.Eq(entity => entity.Id, entity.Id);
        var result = await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellation);

        return result.IsAcknowledged &&
               result.ModifiedCount > 0;
    }

    public virtual async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellation = default)
    {
        await _collection.InsertOneAsync(entity, cancellationToken: cancellation);

        entity.CreatedAt = DateTime.UtcNow;

        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellation = default)
    {
        entity.MarkAsUpdated();

        var filter = Builders<TEntity>.Filter.Eq(entity => entity.Id, entity.Id);

        await _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellation);
        return entity;
    }
}