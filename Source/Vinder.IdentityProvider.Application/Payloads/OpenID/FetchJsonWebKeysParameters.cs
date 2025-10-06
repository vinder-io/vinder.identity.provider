namespace Vinder.IdentityProvider.Application.Payloads.OpenID;

public sealed record FetchJsonWebKeysParameters : IRequest<Result<JsonWebKeySetScheme>>;