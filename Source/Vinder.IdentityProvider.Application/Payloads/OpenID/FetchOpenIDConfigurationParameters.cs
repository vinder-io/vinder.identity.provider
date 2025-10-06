namespace Vinder.IdentityProvider.Application.Payloads.OpenID;

public sealed record FetchOpenIDConfigurationParameters :
    IRequest<Result<OpenIDConfigurationScheme>>;