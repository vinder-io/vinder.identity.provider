namespace Vinder.IdentityProvider.Domain.Filters.Builders;

public sealed class ScopeFiltersBuilder
{
    private readonly ScopeFilters _filters = new();
    public ScopeFilters Build() => _filters;

    public ScopeFiltersBuilder WithScopeId(Guid? id)
    {
        if (id.HasValue && id != Guid.Empty)
        {
            _filters.ScopeId = id;
        }

        return this;
    }

    public ScopeFiltersBuilder WithName(string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            _filters.Name = name;
        }

        return this;
    }

    public ScopeFiltersBuilder WithDescription(string? description)
    {
        if (!string.IsNullOrWhiteSpace(description))
        {
            _filters.Description = description;
        }

        return this;
    }

    public ScopeFiltersBuilder WithIsDeleted(bool? isDeleted)
    {
        if (isDeleted.HasValue)
        {
            _filters.IsDeleted = isDeleted;
        }

        return this;
    }

    public ScopeFiltersBuilder WithPageNumber(int? pageNumber)
    {
        if (pageNumber.HasValue && pageNumber > 0)
        {
            _filters.PageNumber = pageNumber.Value;
        }

        return this;
    }

    public ScopeFiltersBuilder WithPageSize(int? pageSize)
    {
        if (pageSize.HasValue && pageSize > 0)
        {
            _filters.PageSize = pageSize.Value;
        }

        return this;
    }
}
