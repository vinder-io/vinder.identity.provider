namespace Vinder.Federation.Domain.Filtering;

public sealed class TenantFilters : Filters
{
    public string? Name { get; set; }
    public string? ClientId { get; set; }

    public static TenantFiltersBuilder WithSpecifications() => new();
}