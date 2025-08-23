namespace Vinder.IdentityProvider.Application.Handlers.Tenant;

public sealed class TenantCreationHandler(
    ITenantRepository repository,
    IClientCredentialsGenerator credentialsGenerator
) : IRequestHandler<TenantForCreation, Result<TenantDetails>>
{
    public async Task<Result<TenantDetails>> Handle(TenantForCreation request, CancellationToken cancellationToken)
    {
        var filters = new TenantFiltersBuilder()
            .WithName(request.Name)
            .Build();

        var tenants = await repository.GetTenantsAsync(filters, cancellationToken);
        if (tenants.Count > 0)
        {
            return Result<TenantDetails>.Failure(TenantErrors.TenantAlreadyExists);
        }

        var (clientId, clientSecret) = await credentialsGenerator.GenerateAsync(request.Name);

        var tenant = TenantMapper.AsTenant(
            tenant: request,
            clientId: clientId,
            secretHash: clientSecret
        );

        var masterFilters = new TenantFiltersBuilder()
            .WithName("master")
            .Build();

        var matchingTenants = await repository.GetTenantsAsync(masterFilters, cancellationToken);
        var defaultTenant = matchingTenants.FirstOrDefault()!;

        var defaultPermissions = DefaultTenantPermissions.InitialPermissions;

        tenant.Permissions = [.. defaultTenant.Permissions
            .Where(permission => defaultPermissions.Contains(permission.Name))
            .Select(permission => new Domain.Entities.Permission
            {
                Id = permission.Id,
                Name = permission.Name,
                TenantId = tenant.Id,
            })];

        await repository.InsertAsync(tenant, cancellationToken);

        return Result<TenantDetails>.Success(TenantMapper.AsResponse(tenant));
    }
}