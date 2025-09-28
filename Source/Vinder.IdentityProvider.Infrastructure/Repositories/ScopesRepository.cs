namespace Vinder.IdentityProvider.Infrastructure.Repositories;

public sealed class ScopesRepository(IMongoDatabase database) :
    BaseRepository<Scope>(database, Collections.Scopes),
    IScopeRepository
{
    public async Task<IReadOnlyCollection<Scope>> GetScopesAsync(
        ScopeFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Scope>()
            .As<Scope, Scope, BsonDocument>()
            .FilterScopes(filters)
            .Paginate(filters);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var scopes = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<Scope>(bson))
            .ToList();

        return scopes;
    }

    public async Task<long> CountAsync(ScopeFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Scope>()
            .As<Scope, Scope, BsonDocument>()
            .FilterScopes(filters)
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