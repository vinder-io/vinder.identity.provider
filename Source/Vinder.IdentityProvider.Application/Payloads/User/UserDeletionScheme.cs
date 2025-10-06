namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record UserDeletionScheme : IRequest<Result>
{
    public string UserId { get; init; } = default!;
}