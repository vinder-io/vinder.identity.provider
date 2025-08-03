namespace Vinder.IdentityProvider.Domain.Filters.Builders;

public sealed class GroupFiltersBuilder
{
    private readonly GroupFilters _filters = new();

    public GroupFilters Build() => _filters;

    public GroupFiltersBuilder WithId(Guid? id)
    {
        if (id.HasValue && id != Guid.Empty)
        {
            _filters.Id = id;
        }

        return this;
    }

    public GroupFiltersBuilder WithTenantId(Guid? tenantId)
    {
        if (tenantId.HasValue && tenantId != Guid.Empty)
        {
            _filters.TenantId = tenantId;
        }

        return this;
    }

    public GroupFiltersBuilder WithName(string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            _filters.Name = name;
        }

        return this;
    }

    public GroupFiltersBuilder WithPageNumber(int? pageNumber)
    {
        if (pageNumber.HasValue && pageNumber > 0)
        {
            _filters.PageNumber = pageNumber.Value;
        }

        return this;
    }

    public GroupFiltersBuilder WithPageSize(int? pageSize)
    {
        if (pageSize.HasValue && pageSize > 0)
        {
            _filters.PageSize = pageSize.Value;
        }

        return this;
    }

    public GroupFiltersBuilder WithIsDeleted(bool? isDeleted)
    {
        if (isDeleted.HasValue)
        {
            _filters.IsDeleted = isDeleted;
        }

        return this;
    }
}
