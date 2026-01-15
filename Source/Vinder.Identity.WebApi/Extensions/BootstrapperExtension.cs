namespace Vinder.Identity.WebApi.Extensions;

[ExcludeFromCodeCoverage]
public static class BootstrapperExtension
{
    public static async Task UseBootstrapperAsync(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();

        var tenantCollection = scope.ServiceProvider.GetRequiredService<ITenantCollection>();
        var userCollection = scope.ServiceProvider.GetRequiredService<IUserCollection>();
        var scopeRepository = scope.ServiceProvider.GetRequiredService<IScopeCollection>();
        var permissionCollection = scope.ServiceProvider.GetRequiredService<IPermissionCollection>();

        var tenantProvider = scope.ServiceProvider.GetRequiredService<ITenantProvider>();
        var credentialsGenerator = scope.ServiceProvider.GetRequiredService<IClientCredentialsGenerator>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var settings = scope.ServiceProvider.GetRequiredService<ISettings>();

        var tenantCredentials = await credentialsGenerator.GenerateAsync("master", cancellation: default);

        var defaultTenant = new Tenant { Name = "master", ClientId = tenantCredentials.ClientId };
        var tenantFilters = new TenantFiltersBuilder()
            .WithName("master")
            .Build();

        var tenants = await tenantCollection.GetTenantsAsync(tenantFilters, cancellation: default);
        var tenant = tenants.FirstOrDefault();

        if (tenant is not null)
        {
            return;
        }

        defaultTenant.SecretHash = await passwordHasher.HashPasswordAsync(tenantCredentials.ClientId + defaultTenant.Name);
        defaultTenant.Permissions = [
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.CreateGroup, TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.DeleteGroup, TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.ViewGroups,  TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.EditGroup,   TenantId = defaultTenant.Id },

            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.DeleteUser, TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.EditUser,   TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.ViewUsers,  TenantId = defaultTenant.Id },

            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.CreateTenant, TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.DeleteTenant, TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.EditTenant,   TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.ViewTenants,  TenantId = defaultTenant.Id },

            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.CreatePermission,  TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.AssignPermissions, TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.RevokePermissions, TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.ViewPermissions,   TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.EditPermission,    TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.DeletePermission,  TenantId = defaultTenant.Id },

            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.CreateScope,  TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.EditScope,    TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.DeleteScope,  TenantId = defaultTenant.Id },
            new() { Id = Identifier.Generate<Permission>(), Name = Permissions.ViewScopes,   TenantId = defaultTenant.Id },
        ];

        var scopes = new List<Scope>
        {
            new() { Id = Identifier.Generate<Scope>(), Name = Scopes.OpenID.Name,  Description = Scopes.OpenID.Description,  IsGlobal = true },
            new() { Id = Identifier.Generate<Scope>(), Name = Scopes.Profile.Name, Description = Scopes.Profile.Description, IsGlobal = true },
            new() { Id = Identifier.Generate<Scope>(), Name = Scopes.Email.Name,   Description = Scopes.Email.Description,   IsGlobal = true },
            new() { Id = Identifier.Generate<Scope>(), Name = Scopes.Address.Name, Description = Scopes.Address.Description, IsGlobal = true },
            new() { Id = Identifier.Generate<Scope>(), Name = Scopes.Phone.Name,   Description = Scopes.Phone.Description,   IsGlobal = true },
        };

        tenantProvider.SetTenant(defaultTenant);

        await tenantCollection.InsertAsync(defaultTenant);
        await scopeRepository.InsertManyAsync(scopes);
        await permissionCollection.InsertManyAsync(defaultTenant.Permissions);

        var userFilters = new UserFiltersBuilder()
            .WithUsername(settings.Administration.Username)
            .Build();

        var existingUsers = await userCollection.GetUsersAsync(userFilters);
        var rootUser = existingUsers.FirstOrDefault();

        if (rootUser is null)
        {
            rootUser = new User
            {
                Username = settings.Administration.Username,
                TenantId = defaultTenant.Id,
                Permissions = [.. defaultTenant.Permissions],
                PasswordHash = await passwordHasher.HashPasswordAsync(settings.Administration.Password)
            };

            await userCollection.InsertAsync(rootUser);
        }
    }
}
