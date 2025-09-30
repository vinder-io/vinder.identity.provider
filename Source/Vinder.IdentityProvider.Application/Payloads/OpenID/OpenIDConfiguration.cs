namespace Vinder.IdentityProvider.Application.Payloads.OpenID;

public sealed record OpenIDConfiguration
{
    public string Issuer { get; set; } = default!;
    public string AuthorizationEndpoint { get; set; } = default!;
    public string TokenEndpoint { get; set; } = default!;

    public string UserInfoEndpoint { get; set; } = default!;
    public string JwksUri { get; set; } = default!;

    public IEnumerable<string> ResponseTypesSupported { get; set; } = ["code", "token"];
    public IEnumerable<string> SubjectTypesSupported { get; set; } = ["public"];
    public IEnumerable<string> IdTokenSigningAlgValuesSupported { get; set; } = ["RS256"];
}
