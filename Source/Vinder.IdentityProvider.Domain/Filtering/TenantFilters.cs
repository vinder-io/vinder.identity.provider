namespace Vinder.IdentityProvider.Domain.Filtering;

public sealed class TenantFilters : Filters
{
    public string? Name { get; set; }
    public string? ClientId { get; set; }
}