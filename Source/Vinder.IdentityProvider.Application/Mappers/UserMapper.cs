namespace Vinder.IdentityProvider.Application.Mappers;

public static class UserMapper
{
    public static User AsUser(IdentityEnrollmentCredentials credentials, string tenantId) => new()
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

    public static UserDetailsScheme AsResponse(User user) => new()
    {
        Id = user.Id.ToString(),
        Username = user.Username
    };
}