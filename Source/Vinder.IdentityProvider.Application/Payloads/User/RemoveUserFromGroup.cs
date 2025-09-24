namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record RemoveUserFromGroup : IRequest<Result>
{
    public string UserId { get; init; } = default!;
    public string GroupId { get; init; } = default!;
}
