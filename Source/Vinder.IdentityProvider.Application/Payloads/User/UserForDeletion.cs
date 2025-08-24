namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record UserForDeletion : IRequest<Result>
{
    public Guid UserId { get; init; }
}