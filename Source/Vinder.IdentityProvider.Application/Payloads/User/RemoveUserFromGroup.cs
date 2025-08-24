namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record RemoveUserFromGroup : IRequest<Result>
{
    public Guid UserId { get; init; }
    public Guid GroupId { get; init; }
}
