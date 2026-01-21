namespace Vinder.Federation.Application.Payloads.Connect;

public sealed record FetchJsonWebKeysParameters : IMessage<Result<JsonWebKeySetScheme>>;
