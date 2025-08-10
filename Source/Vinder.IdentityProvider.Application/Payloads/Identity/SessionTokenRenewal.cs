namespace Vinder.IdentityProvider.Application.Payloads.Identity;

public sealed record SessionTokenRenewal : IRequest<Result<AuthenticationResult>>
{
    public string RefreshToken { get; init; } = default!;
}