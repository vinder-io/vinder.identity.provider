namespace Vinder.IdentityProvider.Domain.Filters.Builders;

public sealed class UserFiltersBuilder
{
    private readonly UserFilters _filters = new();
    public UserFilters Build() => _filters;

    public UserFiltersBuilder WithTenantId(string? tenantId)
    {
        if (!string.IsNullOrWhiteSpace(tenantId))
        {
            _filters.TenantId = tenantId;
        }

        return this;
    }

    public UserFiltersBuilder WithUserId(string? userId)
    {
        if (!string.IsNullOrWhiteSpace(userId))
        {
            _filters.UserId = userId;
        }

        return this;
    }

    public UserFiltersBuilder WithSecurityToken(string? token)
    {
        if (!string.IsNullOrWhiteSpace(token))
        {
            _filters.SecurityToken = token;
        }

        return this;
    }

    public UserFiltersBuilder WithUsername(string? username)
    {
        if (!string.IsNullOrWhiteSpace(username))
        {
            _filters.Username = username;
        }

        return this;
    }

    public UserFiltersBuilder WithIsDeleted(bool? isDeleted)
    {
        if (isDeleted.HasValue)
        {
            _filters.IsDeleted = isDeleted;
        }

        return this;
    }

    public UserFiltersBuilder WithPageNumber(int? pageNumber)
    {
        if (pageNumber.HasValue && pageNumber > 0)
        {
            _filters.PageNumber = pageNumber.Value;
        }

        return this;
    }

    public UserFiltersBuilder WithPageSize(int? pageSize)
    {
        if (pageSize.HasValue && pageSize > 0)
        {
            _filters.PageSize = pageSize.Value;
        }

        return this;
    }
}
