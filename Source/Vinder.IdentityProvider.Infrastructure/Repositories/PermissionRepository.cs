namespace Vinder.IdentityProvider.Infrastructure.Repositories;

public sealed class PermissionRepository(IMongoDatabase database) :
    BaseRepository<Permission>(database, Collections.Permissions),
    IPermissionRepository
{
    public async Task<IReadOnlyCollection<Permission>> GetPermissionsAsync(PermissionFilters filters, CancellationToken cancellation)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Permission>()
            .As<Permission, Permission, BsonDocument>()
            .FilterPermissions(filters)
            .Paginate(filters);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var permissions = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<Permission>(bson))
            .ToList();

        return permissions;
    }

    public async Task<long> CountAsync(PermissionFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Permission>()
            .As<Permission, Permission, BsonDocument>()
            .FilterPermissions(filters)
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
