namespace Vinder.IdentityProvider.Infrastructure.Pipelines;

public static class UserFiltersStage
{
    public static PipelineDefinition<User, BsonDocument> FilterUsers(
        this PipelineDefinition<User, BsonDocument> pipeline,
        UserFilters filters,
        ITenantProvider tenantProvider)
    {
        var specifications = BuildMatchFilter(filters, tenantProvider);
        return pipeline.Match(specifications);
    }

    private static FilterDefinition<BsonDocument> BuildMatchFilter(UserFilters filters, ITenantProvider tenantProvider)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var filterDefinitions = new List<FilterDefinition<BsonDocument>>
        {
            MatchIfNotEmpty(DocumentFields.User.Username, filters.Username),
            MatchIfNotEmpty(DocumentFields.User.Id, filters.UserId),
            MatchIfNotEmpty(DocumentFields.User.TenantId, tenant.Id)
        };

        if (!filters.IsDeleted.HasValue)
        {
            filterDefinitions.Add(Builders<BsonDocument>.Filter.Eq(DocumentFields.User.IsDeleted, false));
        }
        else
        {
            filterDefinitions.Add(Builders<BsonDocument>.Filter.Eq(DocumentFields.User.IsDeleted, filters.IsDeleted.Value));
        }

        return Builders<BsonDocument>.Filter.And(filterDefinitions);
    }

    private static FilterDefinition<BsonDocument> MatchIfNotEmpty(string field, string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? FilterDefinition<BsonDocument>.Empty
            : Builders<BsonDocument>.Filter.Eq(field, BsonValue.Create(value));
    }
}
