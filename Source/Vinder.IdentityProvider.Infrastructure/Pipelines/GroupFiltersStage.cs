namespace Vinder.IdentityProvider.Infrastructure.Pipelines;

public static class GroupFiltersStage
{
    public static PipelineDefinition<Group, BsonDocument> FilterGroups(
        this PipelineDefinition<Group, BsonDocument> pipeline,
        GroupFilters filters)
    {
        var specifications = BuildMatchFilter(filters);
        return pipeline.Match(specifications);
    }

    private static FilterDefinition<BsonDocument> BuildMatchFilter(GroupFilters filters)
    {
        var filterDefinitions = new List<FilterDefinition<BsonDocument>>
        {
            MatchIfNotEmpty(DocumentFields.Group.Name, filters.Name),
            MatchIfNotEmptyGuid(DocumentFields.Group.Id, filters.Id),
            MatchIfNotEmptyGuid(DocumentFields.Group.TenantId, filters.TenantId),
        };

        if (!filters.IsDeleted.HasValue)
        {
            filterDefinitions.Add(
                Builders<BsonDocument>.Filter.Eq(DocumentFields.Group.IsDeleted, false)
            );
        }
        else
        {
            filterDefinitions.Add(
                Builders<BsonDocument>.Filter.Eq(DocumentFields.Group.IsDeleted, filters.IsDeleted.Value)
            );
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
