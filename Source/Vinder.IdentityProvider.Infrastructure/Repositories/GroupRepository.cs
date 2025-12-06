namespace Vinder.IdentityProvider.Infrastructure.Repositories;

public sealed class GroupRepository(IMongoDatabase database, ITenantProvider tenantProvider) :
    BaseRepository<Group>(database, Collections.Groups),
    IGroupRepository
{
    public async Task<IReadOnlyCollection<Group>> GetGroupsAsync(GroupFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Group>()
            .As<Group, Group, BsonDocument>()
            .FilterGroups(filters, tenantProvider)
            .Paginate(filters.Pagination)
            .Sort(filters.Sort);

        var options = new AggregateOptions { AllowDiskUse = true };
        var aggregation = await _collection.AggregateAsync(pipeline, options, cancellation);

        var bsonDocuments = await aggregation.ToListAsync(cancellation);
        var groups = bsonDocuments
            .Select(bson => BsonSerializer.Deserialize<Group>(bson))
            .ToList();

        return groups;
    }

    public async Task<long> CountAsync(GroupFilters filters, CancellationToken cancellation = default)
    {
        var pipeline = PipelineDefinitionBuilder
            .For<Group>()
            .As<Group, Group, BsonDocument>()
            .FilterGroups(filters, tenantProvider)
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
