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

    public static TenantFilters AsFilters(TenantFetchParameters parameters) => new()
    {
        Id = parameters.Id,
        ClientId = parameters.ClientId,
        Name = parameters.Name,
        PageNumber = parameters.PageNumber,
        PageSize = parameters.PageSize,
        IsDeleted = parameters.IncludeDeleted.HasValue
            ? (bool?)(!parameters.IncludeDeleted.Value)
            : null
    };
}