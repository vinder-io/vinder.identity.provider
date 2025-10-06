namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record RemoveUserFromGroupScheme : IRequest<Result>
{
    public string UserId { get; init; } = default!;
    public string GroupId { get; init; } = default!;
}
