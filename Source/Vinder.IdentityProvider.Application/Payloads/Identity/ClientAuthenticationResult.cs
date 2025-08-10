namespace Vinder.IdentityProvider.Application.Payloads.Identity;

public sealed record ClientAuthenticationResult
{
    public string AccessToken { get; init; } = default!;
}