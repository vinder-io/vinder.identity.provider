namespace Vinder.Federation.Domain.Filtering.Builders;

public sealed class TenantFiltersBuilder :
    FiltersBuilderBase<TenantFilters, TenantFiltersBuilder>
{
    public TenantFiltersBuilder WithName(string? name)
    {
        _filters.Name = name;

        return this;
    }

    public TenantFiltersBuilder WithClientId(string? clientId)
    {
        _filters.ClientId = clientId;

        return this;
    }
}
