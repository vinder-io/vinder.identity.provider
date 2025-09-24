namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record UserForDeletion : IRequest<Result>
{
    public string UserId { get; init; } = default!;
}