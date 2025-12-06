namespace Vinder.IdentityProvider.Infrastructure.Pipelines;

public static class TenantFiltersStage
{
    public static PipelineDefinition<Tenant, BsonDocument> FilterTenants(this PipelineDefinition<Tenant, BsonDocument> pipeline,
        TenantFilters filters)
    {
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.Tenant.Name, filters.Name),
            FilterDefinitions.MatchIfNotEmpty(Documents.Tenant.ClientId, filters.ClientId),
            FilterDefinitions.MatchIfNotEmpty(Documents.Tenant.Id, filters.Id),
            FilterDefinitions.MatchBool(Documents.Tenant.IsDeleted, filters.IsDeleted)
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}
