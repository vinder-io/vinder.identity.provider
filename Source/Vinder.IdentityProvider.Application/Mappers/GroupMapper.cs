namespace Vinder.IdentityProvider.Application.Mappers;

public static class GroupMapper
{
    public static Group AsGroup(GroupForCreation group, Tenant tenant) => new()
    {
        Name = group.Name,
        TenantId = tenant.Id
    };

    public static Group AsGroup(GroupForUpdate payload, Group group)
    {
        group.Name = payload.Name;

        return group;
    }

    public static GroupDetails AsResponse(Group group) => new()
    {
        Id = group.Id.ToString(),
        Name = group.Name,
        Permissions = [.. group.Permissions.Select(group => PermissionMapper.AsResponse(group))]
    };

    public static IReadOnlyCollection<GroupBasicDetails> AsBasicResponse(IEnumerable<Group> groups)
    {
        return [.. groups.Select(GroupMapper.AsBasicResponse)];
    }

    public static GroupBasicDetails AsBasicResponse(Group group) => new()
    {
        Id = group.Id.ToString(),
        Name = group.Name,
    };

    public static GroupFilters AsFilters(GroupsFetchParameters parameters) => new()
    {
        Id = parameters.Id,
        TenantId = parameters.TenantId,
        Name = parameters.Name,
        PageNumber = parameters.PageNumber,
        PageSize = parameters.PageSize,
        IsDeleted = parameters.IncludeDeleted ?? false
    };
}