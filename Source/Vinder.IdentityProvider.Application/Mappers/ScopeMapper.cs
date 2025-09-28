namespace Vinder.IdentityProvider.Application.Mappers;

public static class ScopeMapper
{
    public static Scope AsScope(ScopeForCreation scope, Tenant tenant) => new()
    {
        Name = scope.Name,
        Description = scope.Description,
        TenantId = tenant.Id,
        IsGlobal = false
    };

    public static ScopeDetails AsResponse(Scope scope) => new()
    {
        Id = scope.Id,
        Name = scope.Name,
        Description = scope.Description,
    };
}