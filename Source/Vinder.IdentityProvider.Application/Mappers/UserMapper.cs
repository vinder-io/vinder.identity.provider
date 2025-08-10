namespace Vinder.IdentityProvider.Application.Mappers;

public static class UserMapper
{
    public static User AsUser(IdentityEnrollmentCredentials credentials, Guid tenantId) => new()
    {
        Username = credentials.Username,
        TenantId = tenantId
    };
}