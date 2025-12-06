using Vinder.IdentityProvider.Domain.Filtering;

namespace Vinder.IdentityProvider.Application.Mappers;

public static class PermissionMapper
{
    public static PermissionDetailsScheme AsResponse(Permission permission) => new()
    {
        Id = permission.Id.ToString(),
        Name = permission.Name,
        Description = permission.Description
    };

    public static IReadOnlyCollection<PermissionDetailsScheme> AsResponse(IEnumerable<Permission> permissions)
    {
        return [.. permissions.Select(PermissionMapper.AsResponse)];
    }

    public static PermissionFilters AsFilters(PermissionsFetchParameters parameters) => new()
    {
        Name = parameters.Name,
        PageNumber = parameters.PageNumber,
        PageSize = parameters.PageSize,
        IsDeleted = parameters.IncludeDeleted ?? false
    };

    public static Permission AsPermission(PermissionCreationScheme permission, Tenant tenant) => new()
    {
        Name = permission.Name,
        Description = permission.Description,
        TenantId = tenant.Id
    };

    public static Permission AsPermission(PermissionUpdateScheme payload, Permission permission, Tenant tenant)
    {
        permission.Name = payload.Name;
        permission.Description = payload.Description ?? permission.Description;
        permission.TenantId = tenant.Id;

        permission.MarkAsUpdated();

        return permission;
    }
}