namespace Vinder.IdentityProvider.Application.Payloads.OpenID;

public sealed record JsonWebKeySet
{
    public IEnumerable<JsonWebKeyDetails> Keys { get; set; } = [];
}