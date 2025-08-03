namespace Vinder.IdentityProvider.Infrastructure.Pipelines;

public static class TenantFiltersStage
{
    public static PipelineDefinition<Tenant, BsonDocument> FilterTenants(
        this PipelineDefinition<Tenant, BsonDocument> pipeline,
        TenantFilters filters)
    {
        var specifications = BuildMatchFilter(filters);
        return pipeline.Match(specifications);
    }

    private static FilterDefinition<BsonDocument> BuildMatchFilter(TenantFilters filters)
    {
        var filterDefinitions = new List<FilterDefinition<BsonDocument>>
        {
            MatchIfNotEmpty(DocumentFields.Tenant.Name, filters.Name),
            MatchIfNotEmpty(DocumentFields.Tenant.ClientId, filters.ClientId),
            MatchIfNotEmptyGuid(DocumentFields.Tenant.Id, filters.Id),
        };

        if (!filters.IsDeleted.HasValue)
        {
            filterDefinitions.Add(
                Builders<BsonDocument>.Filter.Eq(DocumentFields.Tenant.IsDeleted, false)
            );
        }
        else
        {
            filterDefinitions.Add(
                Builders<BsonDocument>.Filter.Eq(DocumentFields.Tenant.IsDeleted, filters.IsDeleted.Value)
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
