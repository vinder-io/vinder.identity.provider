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

        var masterTenants = await repository.GetTenantsAsync(masterFilters, cancellationToken);
        var defaultTenant = masterTenants.FirstOrDefault()!;

        tenant.Permissions = [
            new() { Name = Permissions.CreateGroup, TenantId = defaultTenant.Id },
            new() { Name = Permissions.DeleteGroup, TenantId = defaultTenant.Id },
            new() { Name = Permissions.ViewGroups,  TenantId = defaultTenant.Id },
            new() { Name = Permissions.EditGroup,   TenantId = defaultTenant.Id },

            new() { Name = Permissions.EditTenant, TenantId = defaultTenant.Id },

            new() { Name = Permissions.CreatePermission,  TenantId = defaultTenant.Id },
            new() { Name = Permissions.AssignPermissions, TenantId = defaultTenant.Id },
            new() { Name = Permissions.RevokePermissions, TenantId = defaultTenant.Id },
            new() { Name = Permissions.ViewPermissions,   TenantId = defaultTenant.Id },
            new() { Name = Permissions.EditPermission,    TenantId = defaultTenant.Id },
            new() { Name = Permissions.DeletePermission,  TenantId = defaultTenant.Id }
        ];

        await repository.InsertAsync(tenant, cancellationToken);

        return Result<TenantDetails>.Success(TenantMapper.AsResponse(tenant));
    }
}