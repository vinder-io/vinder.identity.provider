namespace Vinder.IdentityProvider.Domain.Filters.Builders;

public sealed class PermissionFiltersBuilder
{
    private readonly PermissionFilters _filters = new();

    public PermissionFilters Build() => _filters;

    public PermissionFiltersBuilder WithName(string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            _filters.Name = name;
        }

        return this;
    }

    public PermissionFiltersBuilder WithTenantId(Guid? tenantId)
    {
        if (tenantId.HasValue && tenantId != Guid.Empty)
        {
            _filters.TenantId = tenantId;
        }

        return this;
    }

    public PermissionFiltersBuilder WithPermissionId(Guid? permissionId)
    {
        if (permissionId.HasValue && permissionId != Guid.Empty)
        {
            _filters.PermissionId = permissionId;
        }

        return this;
    }

    public PermissionFiltersBuilder WithPageNumber(int? pageNumber)
    {
        if (pageNumber.HasValue && pageNumber > 0)
        {
            _filters.PageNumber = pageNumber.Value;
        }

        return this;
    }

    public PermissionFiltersBuilder WithIsDeleted(bool? isDeleted)
    {
        if (isDeleted.HasValue)
        {
            _filters.IsDeleted = isDeleted;
        }

        return this;
    }

    public PermissionFiltersBuilder WithPageSize(int? pageSize)
    {
        if (pageSize.HasValue && pageSize > 0)
        {
            _filters.PageSize = pageSize.Value;
        }

        return this;
    }
}
