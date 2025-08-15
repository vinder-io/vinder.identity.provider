namespace Vinder.IdentityProvider.Application.Mappers;

public static class PermissionMapper
{
    public static PermissionDetails AsResponse(Permission permission) => new()
    {
        Id = permission.Id.ToString(),
        Name = permission.Name,
        Description = permission.Description
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

    public static Permission AsPermission(PermissionForUpdate payload, Permission permission, Tenant tenant)
    {
        permission.Name = payload.Name;
        permission.Description = payload.Description ?? permission.Description;
        permission.TenantId = tenant.Id;
        permission.UpdatedAt = DateTime.Now;

        return permission;
    }
}