namespace Vinder.Identity.Application.Payloads.Identity;

public sealed record AuthenticationCredentials : IMessage<Result<AuthenticationResult>>
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
}