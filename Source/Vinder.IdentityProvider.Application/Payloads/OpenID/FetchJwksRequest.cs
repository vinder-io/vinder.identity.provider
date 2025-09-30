namespace Vinder.IdentityProvider.Application.Payloads.OpenID;

public sealed record FetchJsonWebKeysRequest : IRequest<Result<JsonWebKeySet>>;