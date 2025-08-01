namespace Vinder.IdentityProvider.Domain.Filters.Builders;

public sealed class TenantFiltersBuilder
{
    private readonly TenantFilters _filters = new();
    public TenantFilters Build() => _filters;

    public TenantFiltersBuilder WithId(Guid? id)
    {
        if (id.HasValue && id != Guid.Empty)
        {
            _filters.Id = id;
        }

        return this;
    }

    public TenantFiltersBuilder WithName(string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            _filters.Name = name;
        }

        return this;
    }

    public TenantFiltersBuilder WithClientId(string? clientId)
    {
        if (!string.IsNullOrWhiteSpace(clientId))
        {
            _filters.ClientId = clientId;
        }

        return this;
    }

    public TenantFiltersBuilder WithIsDeleted(bool? isDeleted)
    {
        if (isDeleted.HasValue)
        {
            _filters.IsDeleted = isDeleted;
        }

        return this;
    }

    public TenantFiltersBuilder WithPageNumber(int? pageNumber)
    {
        if (pageNumber.HasValue && pageNumber > 0)
        {
            _filters.PageNumber = pageNumber.Value;
        }

        return this;
    }

    public TenantFiltersBuilder WithPageSize(int? pageSize)
    {
        if (pageSize.HasValue && pageSize > 0)
        {
            _filters.PageSize = pageSize.Value;
        }

        return this;
    }
}
