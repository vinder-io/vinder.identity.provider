namespace Vinder.Identity.Infrastructure.Pipelines;

public static class PermissionFiltersStage
{
    public static PipelineDefinition<Permission, BsonDocument> FilterPermissions(this PipelineDefinition<Permission, BsonDocument> pipeline,
        PermissionFilters filters, ITenantProvider tenantProvider)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var definitions = new List<FilterDefinition<BsonDocument>>
        {
            FilterDefinitions.MatchIfNotEmpty(Documents.Permission.TenantId, tenant.Id),
            FilterDefinitions.MatchIfNotEmpty(Documents.Permission.Id, filters.Id),
            FilterDefinitions.MatchIfNotEmpty(Documents.Permission.Name, filters.Name),
            FilterDefinitions.MatchBool(Documents.Permission.IsDeleted, filters.IsDeleted),
        };

        return pipeline.Match(Builders<BsonDocument>.Filter.And(definitions));
    }
}
