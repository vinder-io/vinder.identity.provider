namespace Vinder.Identity.Application.Handlers.Tenant;

public sealed class TenantCreationHandler(ITenantCollection collection, IClientCredentialsGenerator credentialsGenerator) :
    IRequestHandler<TenantCreationScheme, Result<TenantDetailsScheme>>
{
    public async Task<Result<TenantDetailsScheme>> Handle(
        TenantCreationScheme request, CancellationToken cancellationToken)
    {
        var filters = new TenantFiltersBuilder()
            .WithName(request.Name)
            .Build();

        var tenants = await collection.GetTenantsAsync(filters, cancellationToken);
        if (tenants.Count > 0)
        {
            return Result<TenantDetailsScheme>.Failure(TenantErrors.TenantAlreadyExists);
        }

        var credentials = await credentialsGenerator.GenerateAsync(request.Name, cancellation: cancellationToken);
        var tenant = TenantMapper.AsTenant(
            tenant: request,
            clientId: credentials.ClientId,
            secretHash: credentials.ClientSecret
        );

        var masterFilters = new TenantFiltersBuilder()
            .WithName("master")
            .Build();

        var matchingTenants = await collection.GetTenantsAsync(masterFilters, cancellationToken);
        var defaultTenant = matchingTenants.FirstOrDefault()!;

        tenant.Permissions = defaultTenant.Permissions
            .Where(permission => DefaultTenantPermissions.InitialPermissions.Contains(permission.Name))
            .ToList();

        await collection.InsertAsync(tenant, cancellation: cancellationToken);

        return Result<TenantDetailsScheme>.Success(TenantMapper.AsResponse(tenant));
    }
}