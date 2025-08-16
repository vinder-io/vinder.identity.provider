namespace Vinder.IdentityProvider.Application.Mappers;

public static class GroupMapper
{
    public static Group AsGroup(GroupForCreation group, Tenant tenant) => new()
    {
        Name = group.Name,
        TenantId = tenant.Id
    };

    public static Group AsGroup(GroupForUpdate group, Tenant tenant) => new()
    {
        Name = group.Name,
        TenantId = tenant.Id
    };

    public static GroupDetails AsResponse(Group group) => new()
    {
        Id = group.Id.ToString(),
        Name = group.Name,
        Permissions = [.. group.Permissions.Select(group => PermissionMapper.AsResponse(group))]
    };

    public static GroupFilters AsFilters(GroupsFetchParameters parameters) => new()
    {
        Id = parameters.Id,
        TenantId = parameters.TenantId,
        Name = parameters.Name,
        PageNumber = parameters.PageNumber,
        PageSize = parameters.PageSize,
        IsDeleted = parameters.IncludeDeleted.HasValue
            ? (bool?)(!parameters.IncludeDeleted.Value)
            : null
    };
}