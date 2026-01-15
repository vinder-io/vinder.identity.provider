namespace Vinder.Identity.Application.Payloads.OpenID;

public sealed record FetchJsonWebKeysParameters : IRequest<Result<JsonWebKeySetScheme>>;