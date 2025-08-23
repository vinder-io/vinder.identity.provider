namespace Vinder.IdentityProvider.Application.Mappers;

public static class TenantMapper
{
    public static Tenant AsTenant(TenantForCreation tenant, string clientId, string secretHash) => new()
    {
        Name = tenant.Name,
        Description = tenant.Description,
        ClientId = clientId,
        SecretHash = secretHash
    };

    public static TenantDetails AsResponse(Tenant tenant) => new()
    {
        Id = tenant.Id.ToString(),
        Name = tenant.Name,
        Description = tenant.Description,
        ClientId = tenant.ClientId,
        ClientSecret = tenant.SecretHash
    }; 
}