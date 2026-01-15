namespace Vinder.Identity.Infrastructure.Pipelines;

public static class TokenFiltersStage
{
    public static PipelineDefinition<SecurityToken, BsonDocument> FilterTokens(this PipelineDefinition<SecurityToken, BsonDocument> pipeline,
        TokenFilters filters, ITenantProvider tenantProvider)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmptyEnum(Documents.SecurityToken.Type, filters.Type),
            FilterDefinitions.MatchIfNotEmpty(Documents.SecurityToken.Value, filters.Value),
            FilterDefinitions.MatchIfNotEmpty(Documents.SecurityToken.UserId, filters.UserId),

            FilterDefinitions.MatchIfNotEmpty(Documents.SecurityToken.TenantId, tenant.Id),
            FilterDefinitions.MatchBool(Documents.SecurityToken.IsDeleted, filters.IsDeleted)
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}