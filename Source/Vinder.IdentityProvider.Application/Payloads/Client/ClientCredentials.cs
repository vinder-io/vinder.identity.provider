namespace Vinder.IdentityProvider.Application.Payloads.Client;

public sealed record ClientCredentials
{
    public string ClientId { get; init; } = default!;
    public string ClientSecret { get; init; } = default!;
}