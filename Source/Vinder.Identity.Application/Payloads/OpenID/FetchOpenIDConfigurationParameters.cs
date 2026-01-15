namespace Vinder.Identity.Application.Payloads.OpenID;

public sealed record FetchOpenIDConfigurationParameters :
    IRequest<Result<OpenIDConfigurationScheme>>;