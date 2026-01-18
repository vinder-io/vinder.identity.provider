namespace Vinder.Federation.Application.Payloads.OpenID;

public sealed record FetchJsonWebKeysParameters : IMessage<Result<JsonWebKeySetScheme>>;