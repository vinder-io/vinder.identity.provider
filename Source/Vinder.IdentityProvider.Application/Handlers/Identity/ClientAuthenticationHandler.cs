namespace Vinder.IdentityProvider.Application.Handlers.Identity;

public sealed class ClientAuthenticationHandler(
    ITenantRepository tenantRepository,
    IPasswordHasher passwordHasher,
    ISecurityTokenService tokenService
) : IRequestHandler<ClientAuthenticationCredentials, Result<ClientAuthenticationResult>>
{
    public async Task<Result<ClientAuthenticationResult>> Handle(ClientAuthenticationCredentials request, CancellationToken cancellationToken)
    {
        var filters = new TenantFiltersBuilder()
            .WithClientId(request.ClientId)
            .Build();

        var tenants = await tenantRepository.GetTenantsAsync(filters, cancellation: cancellationToken);
        var tenant = tenants.FirstOrDefault();

        if (tenant is null)
        {
            return Result<ClientAuthenticationResult>.Failure(AuthenticationErrors.ClientNotFound);
        }

        var passwordIsValid = await passwordHasher.VerifyPasswordAsync(request.ClientId + tenant.Name, tenant.SecretHash);
        if (!passwordIsValid)
        {
            return Result<ClientAuthenticationResult>.Failure(AuthenticationErrors.InvalidClientCredentials);
        }

        var tokenResult = await tokenService.GenerateAccessTokenAsync(tenant, cancellationToken);
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