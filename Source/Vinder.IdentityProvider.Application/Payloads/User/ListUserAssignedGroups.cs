namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record ListUserAssignedGroups :
    IRequest<Result<IReadOnlyCollection<GroupBasicDetails>>>
{
    public Guid UserId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
