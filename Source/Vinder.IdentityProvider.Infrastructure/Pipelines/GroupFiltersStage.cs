namespace Vinder.IdentityProvider.Infrastructure.Pipelines;

public static class GroupFiltersStage
{
    public static PipelineDefinition<Group, BsonDocument> FilterGroups(
        this PipelineDefinition<Group, BsonDocument> pipeline,
        GroupFilters filters, ITenantProvider tenantProvider)
    {
        var specifications = BuildMatchFilter(filters, tenantProvider);
        return pipeline.Match(specifications);
    }

    private static FilterDefinition<BsonDocument> BuildMatchFilter(GroupFilters filters, ITenantProvider tenantProvider)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var filterDefinitions = new List<FilterDefinition<BsonDocument>>
        {
            MatchIfNotEmpty(DocumentFields.Group.Name, filters.Name),
            MatchIfNotEmpty(DocumentFields.Group.Id, filters.Id),
            MatchIfNotEmpty(DocumentFields.Group.TenantId, tenant.Id),
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
}
