namespace Vinder.Federation.Application.Payloads.Connect;

public sealed record FetchOpenIDConfigurationParameters :
    IMessage<Result<OpenIDConfigurationScheme>>;