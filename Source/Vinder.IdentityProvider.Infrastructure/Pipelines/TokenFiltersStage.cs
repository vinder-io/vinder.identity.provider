namespace Vinder.IdentityProvider.Infrastructure.Pipelines;

public static class TokenFiltersStage
{
    public static PipelineDefinition<SecurityToken, BsonDocument> FilterTokens(
        this PipelineDefinition<SecurityToken, BsonDocument> pipeline,
        TokenFilters filters,
        ITenantProvider tenantProvider)
    {
        var specifications = BuildMatchFilter(filters, tenantProvider);
        return pipeline.Match(specifications);
    }

    private static FilterDefinition<BsonDocument> BuildMatchFilter(TokenFilters filters, ITenantProvider tenantProvider)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var filterDefinitions = new List<FilterDefinition<BsonDocument>>
        {
            MatchIfNotEmpty(DocumentFields.SecurityToken.Value, filters.Value),
            MatchIfNotEmptyEnum(DocumentFields.SecurityToken.Type, filters.Type),
            MatchIfNotEmptyGuid(DocumentFields.SecurityToken.UserId, filters.UserId),
            MatchIfNotEmptyGuid(DocumentFields.SecurityToken.TenantId, tenant.Id),
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

    private static FilterDefinition<BsonDocument> MatchIfNotEmptyEnum<TEnum>(string field, TEnum? value)
        where TEnum : struct, Enum
    {
        return value.HasValue
            ? Builders<BsonDocument>.Filter.Eq(field, Convert.ToInt32(value.Value))
            : FilterDefinition<BsonDocument>.Empty;
    }
}