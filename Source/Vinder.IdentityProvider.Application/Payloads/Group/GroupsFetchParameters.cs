namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed record GroupsFetchParameters : IRequest<Result<Pagination<GroupDetailsScheme>>>
{
    public string? Id { get; set; }
    public string? TenantId { get; set; }
    public string? Name { get; set; }
    public bool? IncludeDeleted { get; set; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}