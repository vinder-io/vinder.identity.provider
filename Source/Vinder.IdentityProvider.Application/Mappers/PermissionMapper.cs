namespace Vinder.IdentityProvider.Application.Mappers;

public static class PermissionMapper
{
    public static PermissionDetails AsResponse(Permission permission) => new()
    {
        Id = permission.Id.ToString(),
        Name = permission.Name
    };

    public static PermissionFilters AsFilters(PermissionsFetchParameters parameters) => new()
    {
        Name = parameters.Name,
        PageNumber = parameters.PageNumber,
        PageSize = parameters.PageSize,
        IsDeleted = parameters.IncludeDeleted.HasValue
            ? (bool?)(!parameters.IncludeDeleted.Value)
            : null
    };

    public static Permission AsPermission(PermissionForCreation permission, Tenant tenant) => new()
    {
        Name = permission.Name,
        Description = permission.Description,
        TenantId = tenant.Id
    };
}