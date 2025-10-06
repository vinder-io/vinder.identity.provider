namespace Vinder.IdentityProvider.Application.Payloads.Identity;

public sealed record IdentityEnrollmentCredentials : IRequest<Result<UserDetailsScheme>>
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
}