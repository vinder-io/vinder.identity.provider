namespace Vinder.IdentityProvider.Application.Payloads.Identity;

public sealed record IdentityEnrollmentCredentials : IRequest<Result<UserDetails>>
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
}