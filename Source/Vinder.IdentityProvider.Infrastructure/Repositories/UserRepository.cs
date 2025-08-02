namespace Vinder.IdentityProvider.Infrastructure.Repositories;

public sealed class UserRepository(IMongoDatabase database) :
    BaseRepository<User>(database, Collections.Users),
    IUserRepository
{
    public async Task<IReadOnlyCollection<User>> GetUsersAsync(UserFilters filters, CancellationToken cancellation)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<User>()
            .As<User, User, BsonDocument>()
            .FilterUsers(filters)
            .Paginate(filters);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var users = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<User>(bson))
            .ToList();

        return users;
    }

    public async Task<long> CountAsync(UserFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<User>()
            .As<User, User, BsonDocument>()
            .FilterUsers(filters)
            .Count();

        var aggregation = await _collection.AggregateAsync(pipeline, cancellationToken: cancellation);
        var result = await aggregation.FirstOrDefaultAsync(cancellation);

        if (result == null)
        {
            return 0;
        }

        return result.Count;
    }
}
