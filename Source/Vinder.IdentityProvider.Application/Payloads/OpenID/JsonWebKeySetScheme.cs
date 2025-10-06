namespace Vinder.IdentityProvider.Application.Payloads.OpenID;

public sealed record JsonWebKeySetScheme
{
    public IEnumerable<JsonWebKeyScheme> Keys { get; set; } = [];
}