namespace Vinder.IdentityProvider.Application.Mappers;

public static class UserMapper
{
    public static User AsUser(IdentityEnrollmentCredentials credentials, Guid tenantId) => new()
    {
        Username = credentials.Username,
        TenantId = tenantId,
    };

    public static UserFilters AsFilters(UsersFetchParameters parameters) => new()
    {
        UserId = parameters.Id,
        Username = parameters.Username,
        IsDeleted = parameters.IsDeleted,
        PageNumber = parameters.PageNumber,
        PageSize = parameters.PageSize
    };

    public static UserDetails AsResponse(User user) => new()
    {
        Id = user.Id.ToString(),
        Username = user.Username,
        Groups = [.. user.Groups.Select(GroupMapper.AsResponse)],
        Permissions = [.. user.Permissions.Select(PermissionMapper.AsResponse)]
    };
}