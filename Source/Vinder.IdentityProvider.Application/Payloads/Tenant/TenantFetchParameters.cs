namespace Vinder.IdentityProvider.Application.Payloads.Tenant;

public sealed record TenantFetchParameters : IRequest<Result<Pagination<TenantDetails>>>
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? ClientId { get; init; }
    public bool? IncludeDeleted { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}