namespace Vinder.Identity.Application.Handlers.Scope;

public sealed class ScopeCreationHandler(IScopeCollection scopeCollection, ITenantCollection tenantCollection, ITenantProvider tenantProvider) :
    IMessageHandler<ScopeCreationScheme, Result<ScopeDetailsScheme>>
{
    public async Task<Result<ScopeDetailsScheme>> HandleAsync(ScopeCreationScheme parameters, CancellationToken cancellation)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var filters = new ScopeFiltersBuilder()
            .WithName(parameters.Name)
            .Build();

        var scopes = await scopeCollection.GetScopesAsync(filters, cancellation: cancellation);
        var existingScope = scopes.FirstOrDefault();

        if (existingScope is not null)
        {
            return Result<ScopeDetailsScheme>.Failure(ScopeErrors.ScopeAlreadyExists);
        }

        var scope = await scopeCollection.InsertAsync(ScopeMapper.AsScope(parameters, tenant), cancellation: cancellation);
        var response = ScopeMapper.AsResponse(scope);

        tenant.Scopes.Add(scope);

        await tenantCollection.UpdateAsync(tenant, cancellation: cancellation);

        return Result<ScopeDetailsScheme>.Success(response);
    }
}