namespace Vinder.Identity.Application.Handlers.Authorization;

public sealed class AuthorizationHandler(ITenantCollection tenantCollection) :
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
            return Result<AuthorizationScheme>.Failure(TenantErrors.TenantDoesNotExist);

        return Result<AuthorizationScheme>.Success(new());
    }
}