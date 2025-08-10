namespace Vinder.IdentityProvider.Application.Mappers;

public static class GroupMapper
{
    public static Group AsGroup(GroupForCreation group, Tenant tenant) => new()
    {
        Name = group.Name,
        TenantId = tenant.Id
    };

    public static GroupDetails AsResponse(Group group) => new()
    {
        Id = group.Id.ToString(),
        Name = group.Name
    };
}