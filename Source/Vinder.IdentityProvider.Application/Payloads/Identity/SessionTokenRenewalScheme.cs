namespace Vinder.IdentityProvider.Application.Payloads.Identity;

public sealed record SessionTokenRenewalScheme : IRequest<Result<AuthenticationResult>>
{
    public string RefreshToken { get; init; } = default!;
}