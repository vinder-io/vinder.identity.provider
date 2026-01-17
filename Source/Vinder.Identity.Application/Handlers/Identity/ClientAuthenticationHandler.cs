namespace Vinder.Identity.Application.Handlers.Identity;

public sealed class ClientAuthenticationHandler(
    ITenantCollection tenantCollection,
    ISecurityTokenService tokenService
) : IMessageHandler<ClientAuthenticationCredentials, Result<ClientAuthenticationResult>>
{
    public async Task<Result<ClientAuthenticationResult>> HandleAsync(
        ClientAuthenticationCredentials parameters, CancellationToken cancellation = default)
    {
        var filters = TenantFilters.WithSpecifications()
            .WithClientId(parameters.ClientId)
            .Build();

        var tenants = await tenantCollection.GetTenantsAsync(filters, cancellation: cancellation);
        var tenant = tenants.FirstOrDefault();

        if (tenant is null)
        {
            return Result<ClientAuthenticationResult>.Failure(AuthenticationErrors.ClientNotFound);
        }

        if (parameters.ClientSecret != tenant.SecretHash)
        {
            return Result<ClientAuthenticationResult>.Failure(AuthenticationErrors.InvalidClientCredentials);
        }

        var tokenResult = await tokenService.GenerateAccessTokenAsync(tenant, cancellation);
        if (tokenResult.IsFailure)
        {
            return Result<ClientAuthenticationResult>.Failure(tokenResult.Error);
        }

        var response = new ClientAuthenticationResult
        {
            AccessToken = tokenResult.Data!.Value
        };

        return Result<ClientAuthenticationResult>.Success(response);
    }
}