namespace Vinder.Identity.Application.Handlers.Identity;

public sealed class ClientAuthenticationHandler(ITenantCollection tenantCollection, ITokenCollection tokenCollection, ISecurityTokenService tokenService) :
    IMessageHandler<ClientAuthenticationCredentials, Result<ClientAuthenticationResult>>
{
    public async Task<Result<ClientAuthenticationResult>> HandleAsync(
        ClientAuthenticationCredentials parameters, CancellationToken cancellation = default)
    {
        IAuthorizationFlowHandler handler = parameters.GrantType switch
        {
            SupportedGrantType.AuthorizationCode => new AuthorizationCodeGrantHandler(tenantCollection, tokenService, tokenCollection),
            SupportedGrantType.ClientCredentials => new ClientCredentialsGrantHandler(tenantCollection, tokenService),
        };

        return await handler.HandleAsync(parameters, cancellation);
    }
}