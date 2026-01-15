namespace Vinder.Identity.Application.Payloads.Identity;

public sealed record ClientAuthenticationResult
{
    [property: JsonPropertyName("access_token")]
    public string AccessToken { get; init; } = default!;
}