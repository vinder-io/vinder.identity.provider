namespace Vinder.Federation.Application.Handlers.Authorization;

public sealed class AuthorizationHandler(ITenantCollection tenantCollection, IRedirectUriPolicy redirectUriPolicy) :
    IMessageHandler<AuthorizationParameters, Result<AuthorizationScheme>>
{
    public async Task<Result<AuthorizationScheme>> HandleAsync(
        AuthorizationParameters parameters, CancellationToken cancellation = default)
    {
        var filters = new TenantFiltersBuilder()
            .WithClientId(parameters.ClientId)
            .Build();

        var clients = await tenantCollection.GetTenantsAsync(filters, cancellation);
        var client = clients.FirstOrDefault();

        if (client is null)
        {
            return Result<AuthorizationScheme>.Failure(TenantErrors.TenantDoesNotExist);
        }

        var redirectProof = await redirectUriPolicy.EnsureRedirectUriIsAllowedAsync(client, redirectUri, cancellation);
        if (redirectProof.IsFailure)
        {
            return Result<AuthorizationScheme>.Failure(redirectProof.Error);
        }

        return Result<AuthorizationScheme>.Success(parameters.AsReponse());
    }
}
