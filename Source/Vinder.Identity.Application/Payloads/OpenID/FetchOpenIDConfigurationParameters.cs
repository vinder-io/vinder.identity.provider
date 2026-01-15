namespace Vinder.Identity.Application.Payloads.OpenID;

public sealed record FetchOpenIDConfigurationParameters :
    IMessage<Result<OpenIDConfigurationScheme>>;