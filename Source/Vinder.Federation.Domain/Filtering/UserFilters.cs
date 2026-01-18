namespace Vinder.Federation.Domain.Filtering;

public sealed class UserFilters : Filters
{
    public string? TenantId { get; set; }
    public string? SecurityToken { get; set; }
    public string? Username { get; set; }

    public static UserFiltersBuilder WithSpecifications() => new();
}
