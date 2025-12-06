namespace Vinder.IdentityProvider.Domain.Filtering.Builders;

public sealed class ScopeFiltersBuilder : FiltersBuilderBase<ScopeFilters, ScopeFiltersBuilder>
{
    public ScopeFiltersBuilder WithName(string? name)
    {
        _filters.Name = name;

        return this;
    }

    public ScopeFiltersBuilder WithTenantId(string? tenantId)
    {
        _filters.TenantId = tenantId;

        return this;
    }

    public ScopeFiltersBuilder WithDescription(string? description)
    {
        _filters.Description = description;

        return this;
    }
}
