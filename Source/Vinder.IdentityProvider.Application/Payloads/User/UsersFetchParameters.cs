namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record UsersFetchParameters : IRequest<Result<Pagination<UserDetails>>>
{
    public string? Id { get; init; }
    public string? Username { get; init; }
    public bool? IsDeleted { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}