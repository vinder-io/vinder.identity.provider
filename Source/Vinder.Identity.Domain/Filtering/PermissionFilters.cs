namespace Vinder.Identity.Domain.Filtering;

public sealed class PermissionFilters : Filters
{
    public string? TenantId { get; set; }
    public string? Name { get; set; }

    public static PermissionFiltersBuilder WithSpecifications() => new();
}