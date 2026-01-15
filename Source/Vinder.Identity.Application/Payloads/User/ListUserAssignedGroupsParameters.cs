namespace Vinder.Identity.Application.Payloads.User;

public sealed record ListUserAssignedGroupsParameters :
    IRequest<Result<IReadOnlyCollection<GroupBasicDetailsScheme>>>
{
    public string UserId { get; init; } = default!;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
