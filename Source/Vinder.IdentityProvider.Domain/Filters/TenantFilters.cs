namespace Vinder.IdentityProvider.Domain.Filters;

public sealed class TenantFilters : PaginationFilters
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? ClientId { get; set; }
    public bool? IsDeleted { get; set; }
}