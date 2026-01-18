namespace Vinder.Federation.Infrastructure.Pipelines;

public static class GroupFiltersStage
{
    public static PipelineDefinition<Group, BsonDocument> FilterGroups(this PipelineDefinition<Group, BsonDocument> pipeline,
        GroupFilters filters, ITenantProvider tenantProvider)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.Group.Id, filters.Id),
            FilterDefinitions.MatchIfNotEmpty(Documents.Group.Name, filters.Name),
            FilterDefinitions.MatchIfNotEmpty(Documents.Group.TenantId, tenant.Id),
            FilterDefinitions.MatchBool(Documents.Group.IsDeleted, filters.IsDeleted),
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}
