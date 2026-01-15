namespace Vinder.Identity.Application.Handlers.Tenant;

public sealed class TenantCreationHandler(ITenantCollection collection, IClientCredentialsGenerator credentialsGenerator) :
    IMessageHandler<TenantCreationScheme, Result<TenantDetailsScheme>>
{
    public async Task<Result<TenantDetailsScheme>> HandleAsync(
        TenantCreationScheme parameters, CancellationToken cancellation)
    {
        var filters = new TenantFiltersBuilder()
            .WithName(parameters.Name)
            .Build();

        var tenants = await collection.GetTenantsAsync(filters, cancellation);
        if (tenants.Count > 0)
        {
            return Result<TenantDetailsScheme>.Failure(TenantErrors.TenantAlreadyExists);
        }

        var credentials = await credentialsGenerator.GenerateAsync(parameters.Name, cancellation: cancellation);
        var tenant = TenantMapper.AsTenant(
            tenant: parameters,
            clientId: credentials.ClientId,
            secretHash: credentials.ClientSecret
        );

        var masterFilters = new TenantFiltersBuilder()
            .WithName("master")
            .Build();

        var matchingTenants = await collection.GetTenantsAsync(masterFilters, cancellation);
        var defaultTenant = matchingTenants.FirstOrDefault()!;

        tenant.Permissions = defaultTenant.Permissions
            .Where(permission => DefaultTenantPermissions.InitialPermissions.Contains(permission.Name))
            .ToList();

        await collection.InsertAsync(tenant, cancellation: cancellation);

        return Result<TenantDetailsScheme>.Success(TenantMapper.AsResponse(tenant));
    }
}