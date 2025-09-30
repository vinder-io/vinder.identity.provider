namespace Vinder.IdentityProvider.Application.Payloads.OpenID;

public sealed record FetchOpenIDConfigurationRequest :
    IRequest<Result<OpenIDConfiguration>>;