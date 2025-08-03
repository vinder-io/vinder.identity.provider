namespace Vinder.IdentityProvider.Infrastructure.Pipelines;

public static class TokenFiltersStage
{
    public static PipelineDefinition<SecurityToken, BsonDocument> FilterTokens(
        this PipelineDefinition<SecurityToken, BsonDocument> pipeline,
        TokenFilters filters)
    {
        var specifications = BuildMatchFilter(filters);
        return pipeline.Match(specifications);
    }

    private static FilterDefinition<BsonDocument> BuildMatchFilter(TokenFilters filters)
    {
        var filterDefinitions = new List<FilterDefinition<BsonDocument>>
        {
            MatchIfNotEmptyGuid(DocumentFields.SecurityToken.UserId, filters.UserId),
            MatchIfNotEmptyGuid(DocumentFields.SecurityToken.TenantId, filters.TenantId)
        };

        if (!filters.IsDeleted.HasValue)
        {
            filterDefinitions.Add(Builders<BsonDocument>.Filter.Eq(DocumentFields.SecurityToken.IsDeleted, false));
        }
        else
        {
            filterDefinitions.Add(Builders<BsonDocument>.Filter.Eq(DocumentFields.SecurityToken.IsDeleted, filters.IsDeleted.Value));
        }

        return Builders<BsonDocument>.Filter.And(filterDefinitions);
    }

    private static FilterDefinition<BsonDocument> MatchIfNotEmptyGuid(string field, Guid? value)
    {
        return !value.HasValue || value == Guid.Empty
            ? FilterDefinition<BsonDocument>.Empty
            : Builders<BsonDocument>.Filter.Eq(field, new BsonBinaryData(value.Value, GuidRepresentation.Standard));
    }
}