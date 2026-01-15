namespace Vinder.Identity.Infrastructure.Pipelines;

public static class UserFiltersStage
{
    public static PipelineDefinition<User, BsonDocument> FilterUsers(this PipelineDefinition<User, BsonDocument> pipeline,
        UserFilters filters, ITenantProvider tenantProvider)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.User.Id, filters.Id),
            FilterDefinitions.MatchIfNotEmpty(Documents.User.Username, filters.Username),
            FilterDefinitions.MatchIfNotEmpty(Documents.User.TenantId, tenant.Id),
            FilterDefinitions.MatchBool(Documents.User.IsDeleted, filters.IsDeleted)
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}
