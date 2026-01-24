namespace Vinder.Federation.Application.Handlers.Identity;

public sealed class ClientAuthenticationHandler(ITenantCollection tenantCollection, IUserCollection userCollection, ITokenCollection tokenCollection, ISecurityTokenService tokenService) :
    IMessageHandler<ClientAuthenticationCredentials, Result<ClientAuthenticationResult>>
{
    public async Task<Result<ClientAuthenticationResult>> HandleAsync(
        ClientAuthenticationCredentials parameters, CancellationToken cancellation = default)
    {
        IAuthorizationFlowHandler handler = parameters.GrantType switch
        {
            SupportedGrantType.AuthorizationCode => new AuthorizationCodeGrantHandler(tenantCollection, userCollection, tokenService, tokenCollection),
            SupportedGrantType.ClientCredentials => new ClientCredentialsGrantHandler(tenantCollection, tokenService),
        };

        return await handler.HandleAsync(parameters, cancellation);
    }
}
