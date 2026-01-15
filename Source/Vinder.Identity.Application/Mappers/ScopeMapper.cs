namespace Vinder.Identity.Application.Mappers;

public static class ScopeMapper
{
    public static Scope AsScope(ScopeCreationScheme scope, Tenant tenant) => new()
    {
        Name = scope.Name,
        Description = scope.Description,
        TenantId = tenant.Id,
        IsGlobal = false
    };

    public static ScopeDetailsScheme AsResponse(Scope scope) => new()
    {
        Id = scope.Id,
        Name = scope.Name,
        Description = scope.Description,
    };
}