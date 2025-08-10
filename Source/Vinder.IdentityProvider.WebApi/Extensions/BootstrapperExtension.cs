namespace Vinder.IdentityProvider.WebApi.Extensions;

[ExcludeFromCodeCoverage]
public static class BootstrapperExtension
{
    public static async Task UseBootstrapperAsync(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();

        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var tenantRepository = scope.ServiceProvider.GetRequiredService<ITenantRepository>();
        var tenantProvider = scope.ServiceProvider.GetRequiredService<ITenantProvider>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var settings = scope.ServiceProvider.GetRequiredService<ISettings>();

        var tenantName = settings.Administration.Tenant;
        var username = settings.Administration.Username;
        var rawPassword = settings.Administration.Password;

        var defaultTenant = new Tenant {  Name = tenantName };
        var tenantFilters = new TenantFiltersBuilder()
            .WithName(tenantName)
            .Build();

        var tenants = await tenantRepository.GetTenantsAsync(tenantFilters, cancellation: default);
        var tenant = tenants.FirstOrDefault();

        if (tenant is not null)
        {
            return;
        }

        tenantProvider.SetTenant(defaultTenant);

        var filters = new UserFiltersBuilder()
            .WithUsername(username)
            .Build();

        var existingUsers = await userRepository.GetUsersAsync(filters, cancellation: default);

        #pragma warning disable S3626
        if (existingUsers.Count > 0)
        {
            return;
        }

        var hashedPassword = await passwordHasher.HashPasswordAsync(rawPassword);
        var user = new User
        {
            Username = username,
            TenantId = defaultTenant.Id,
            PasswordHash = hashedPassword,
            CreatedAt = DateTime.UtcNow,
        };

        await userRepository.InsertAsync(user);
        await tenantRepository.InsertAsync(defaultTenant);
    }
}
