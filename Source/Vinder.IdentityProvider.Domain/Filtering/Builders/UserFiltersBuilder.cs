namespace Vinder.IdentityProvider.Domain.Filtering.Builders;

public sealed class UserFiltersBuilder : FiltersBuilderBase<UserFilters, UserFiltersBuilder>
{
    public UserFiltersBuilder WithTenantId(string? tenantId)
    {
        _filters.TenantId = tenantId;

        return this;
    }

    public UserFiltersBuilder WithSecurityToken(string? token)
    {
        _filters.SecurityToken = token;

        return this;
    }

    public UserFiltersBuilder WithUsername(string? username)
    {
        _filters.Username = username;
        return this;
    }
}
