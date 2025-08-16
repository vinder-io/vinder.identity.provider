namespace Vinder.IdentityProvider.WebApi.Extensions;

[ExcludeFromCodeCoverage]
public static class BootstrapperExtension
{
    public static async Task UseBootstrapperAsync(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();

        var tenantRepository = scope.ServiceProvider.GetRequiredService<ITenantRepository>();
        var permissionRepository = scope.ServiceProvider.GetRequiredService<IPermissionRepository>();
        var tenantProvider = scope.ServiceProvider.GetRequiredService<ITenantProvider>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        var defaultTenant = new Tenant { Name = "master", ClientId = GenerateClientId() };
        var tenantFilters = new TenantFiltersBuilder()
            .WithName("master")
            .Build();

        var tenants = await tenantRepository.GetTenantsAsync(tenantFilters, cancellation: default);
        var tenant = tenants.FirstOrDefault();

        if (tenant is not null)
        {
            return;
        }

        defaultTenant.SecretHash = await passwordHasher.HashPasswordAsync(defaultTenant.ClientId + defaultTenant.Name);
        defaultTenant.Permissions = [
            new() { Name = Permissions.CreateGroup, TenantId = defaultTenant.Id },
            new() { Name = Permissions.DeleteGroup, TenantId = defaultTenant.Id },
            new() { Name = Permissions.ViewGroups,  TenantId = defaultTenant.Id },
            new() { Name = Permissions.EditGroup,   TenantId = defaultTenant.Id },

            new() { Name = Permissions.CreateClient, TenantId = defaultTenant.Id },
            new() { Name = Permissions.DeleteClient, TenantId = defaultTenant.Id },
            new() { Name = Permissions.EditClient,   TenantId = defaultTenant.Id },
            new() { Name = Permissions.ViewClients,  TenantId = defaultTenant.Id },

            new() { Name = Permissions.CreateTenant, TenantId = defaultTenant.Id },
            new() { Name = Permissions.DeleteTenant, TenantId = defaultTenant.Id },
            new() { Name = Permissions.EditTenant,   TenantId = defaultTenant.Id },
            new() { Name = Permissions.ViewTenants,  TenantId = defaultTenant.Id },

            new() { Name = Permissions.CreatePermission,  TenantId = defaultTenant.Id },
            new() { Name = Permissions.AssignPermissions, TenantId = defaultTenant.Id },
            new() { Name = Permissions.RevokePermissions, TenantId = defaultTenant.Id },
            new() { Name = Permissions.ViewPermissions,   TenantId = defaultTenant.Id },
            new() { Name = Permissions.EditPermission,    TenantId = defaultTenant.Id },
            new() { Name = Permissions.DeletePermission,  TenantId = defaultTenant.Id }
        ];

        tenantProvider.SetTenant(defaultTenant);

        await tenantRepository.InsertAsync(defaultTenant);
        await permissionRepository.InsertManyAsync(defaultTenant.Permissions);
    }

    public static string GenerateClientId(int size = 32)
    {
        var bytes = new byte[size];

        RandomNumberGenerator.Fill(bytes);

        return Convert
            .ToHexString(bytes)
            .ToLowerInvariant();
    }
}
