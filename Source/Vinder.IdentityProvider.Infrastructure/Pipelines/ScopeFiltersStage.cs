namespace Vinder.IdentityProvider.Infrastructure.Pipelines;

public static class ScopeFiltersStage
{
    public static PipelineDefinition<Scope, BsonDocument> FilterScopes(
        this PipelineDefinition<Scope, BsonDocument> pipeline, ScopeFilters filters)
    {
        var specifications = ScopeFiltersStage.BuildMatchFilter(filters);
        return pipeline.Match(specifications);
    }

    private static FilterDefinition<BsonDocument> BuildMatchFilter(ScopeFilters filters)
    {
        var filterDefinitions = new List<FilterDefinition<BsonDocument>>
        {
            MatchIfNotEmpty(DocumentFields.Scope.Name, filters.Name),
            MatchIfNotEmptyGuid(DocumentFields.Scope.Id, filters.ScopeId)
        };

        if (!filters.IsDeleted.HasValue)
        {
            filterDefinitions.Add(Builders<BsonDocument>.Filter.Eq(DocumentFields.Scope.IsDeleted, false));
        }
        else
        {
            filterDefinitions.Add(Builders<BsonDocument>.Filter.Eq(DocumentFields.Scope.IsDeleted, filters.IsDeleted.Value));
        }

        return Builders<BsonDocument>.Filter.And(filterDefinitions);
    }

    private static FilterDefinition<BsonDocument> MatchIfNotEmpty(string field, string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? FilterDefinition<BsonDocument>.Empty
            : Builders<BsonDocument>.Filter.Eq(field, BsonValue.Create(value));
    }

    private static FilterDefinition<BsonDocument> MatchIfNotEmptyGuid(string field, Guid? value)
    {
        return !value.HasValue || value == Guid.Empty
            ? FilterDefinition<BsonDocument>.Empty
            : Builders<BsonDocument>.Filter.Eq(field, new BsonBinaryData(value.Value, GuidRepresentation.Standard));
    }
}
