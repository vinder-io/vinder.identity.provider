namespace Vinder.Identity.Application.Payloads.OpenID;

public sealed record FetchJsonWebKeysParameters : IMessage<Result<JsonWebKeySetScheme>>;