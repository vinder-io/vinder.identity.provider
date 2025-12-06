namespace Vinder.IdentityProvider.Application.Mappers;

public static class TenantMapper
{
    public static Tenant AsTenant(TenantCreationScheme tenant, string clientId, string secretHash) => new()
    {
        Name = tenant.Name,
        Description = tenant.Description,
        ClientId = clientId,
        SecretHash = secretHash
    };

    public static Tenant AsTenant(TenantUpdateScheme payload, Tenant tenant)
    {
        tenant.Name = payload.Name;
        tenant.Description = payload.Description ?? tenant.Description;

        tenant.MarkAsUpdated();

        return tenant;
    }

    public static TenantDetailsScheme AsResponse(Tenant tenant) => new()
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
        Pagination = parameters.Pagination,
        Sort = parameters.Sort,
        IsDeleted = parameters.IsDeleted
    };
}