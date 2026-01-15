namespace Vinder.Identity.Application.Handlers.Scope;

public sealed class ScopeCreationHandler(IScopeCollection scopeCollection, ITenantCollection tenantCollection, ITenantProvider tenantProvider) :
    IRequestHandler<ScopeCreationScheme, Result<ScopeDetailsScheme>>
{
    public async Task<Result<ScopeDetailsScheme>> Handle(ScopeCreationScheme request, CancellationToken cancellationToken)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var filters = new ScopeFiltersBuilder()
            .WithName(request.Name)
            .Build();

        var scopes = await scopeCollection.GetScopesAsync(filters, cancellation: cancellationToken);
        var existingScope = scopes.FirstOrDefault();

        if (existingScope is not null)
        {
            return Result<ScopeDetailsScheme>.Failure(ScopeErrors.ScopeAlreadyExists);
        }

        var scope = await scopeCollection.InsertAsync(ScopeMapper.AsScope(request, tenant), cancellation: cancellationToken);
        var response = ScopeMapper.AsResponse(scope);

        tenant.Scopes.Add(scope);

        await tenantCollection.UpdateAsync(tenant, cancellation: cancellationToken);

        return Result<ScopeDetailsScheme>.Success(response);
    }
}