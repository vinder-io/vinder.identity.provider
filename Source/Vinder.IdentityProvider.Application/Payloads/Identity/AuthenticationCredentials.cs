namespace Vinder.IdentityProvider.Application.Payloads.Identity;

public sealed record AuthenticationCredentials : IRequest<Result<AuthenticationResult>>
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
}