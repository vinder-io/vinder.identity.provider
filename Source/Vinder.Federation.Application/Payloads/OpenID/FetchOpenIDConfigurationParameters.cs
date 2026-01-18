namespace Vinder.Federation.Application.Payloads.OpenID;

public sealed record FetchOpenIDConfigurationParameters :
    IMessage<Result<OpenIDConfigurationScheme>>;