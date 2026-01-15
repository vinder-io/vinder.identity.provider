namespace Vinder.Identity.Application.Payloads.Identity;

public sealed record IdentityEnrollmentCredentials : IMessage<Result<UserDetailsScheme>>
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
}