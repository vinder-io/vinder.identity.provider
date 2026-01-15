namespace Vinder.Identity.Domain.Filtering.Builders;

public sealed class GroupFiltersBuilder :
    FiltersBuilderBase<GroupFilters, GroupFiltersBuilder>
{
    public GroupFiltersBuilder WithTenantId(string? tenantId)
    {
        _filters.TenantId = tenantId;

        return this;
    }

    public GroupFiltersBuilder WithName(string? name)
    {
        _filters.Name = name;

        return this;
    }
}
