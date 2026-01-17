namespace Vinder.Identity.Domain.Filtering;

public sealed class GroupFilters : Filters
{
    public string? TenantId { get; set; }
    public string? Name { get; set; }

    public static GroupFiltersBuilder WithSpecifications() => new();
}
