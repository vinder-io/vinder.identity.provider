namespace Vinder.IdentityProvider.Infrastructure.Pipelines;

public static class UserFiltersStage
{
    public static PipelineDefinition<User, BsonDocument> FilterUsers(
        this PipelineDefinition<User, BsonDocument> pipeline,
        UserFilters filters)
    {
        var specifications = BuildMatchFilter(filters);
        return pipeline.Match(specifications);
    }

    private static FilterDefinition<BsonDocument> BuildMatchFilter(UserFilters filters)
    {
        var filterDefinitions = new List<FilterDefinition<BsonDocument>>
        {
            MatchIfNotEmpty(DocumentFields.User.Username, filters.Username),
            MatchIfNotEmptyGuid(DocumentFields.User.Id, filters.UserId),
            MatchIfNotEmptyGuid(DocumentFields.User.TenantId, filters.TenantId)
        };

        if (filters.IsDeleted.HasValue && filters.IsDeleted is true)
        {
            filterDefinitions.Add(Builders<BsonDocument>.Filter.Eq(DocumentFields.User.IsDeleted, true));
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
