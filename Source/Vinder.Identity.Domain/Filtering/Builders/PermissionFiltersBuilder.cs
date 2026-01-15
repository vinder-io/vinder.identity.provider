namespace Vinder.Identity.Domain.Filtering.Builders;

public sealed class PermissionFiltersBuilder :
    FiltersBuilderBase<PermissionFilters, PermissionFiltersBuilder>
{
    public PermissionFiltersBuilder WithName(string? name)
    {
        _filters.Name = name;

        return this;
    }

    public PermissionFiltersBuilder WithTenantId(string? tenantId)
    {
        _filters.TenantId = tenantId;

        return this;
    }
}
