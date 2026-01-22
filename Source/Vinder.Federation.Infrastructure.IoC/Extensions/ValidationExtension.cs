namespace Vinder.Federation.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage]
public static class ValidationExtension
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation(options =>
        {
            options.DisableDataAnnotationsValidation = true;
        });

        services.AddTransient<IValidator<AuthenticationCredentials>, AuthenticationCredentialsValidator>();
        services.AddTransient<IValidator<AuthorizationParameters>, AuthorizationParametersValidator>();
        services.AddTransient<IValidator<ClientAuthenticationCredentials>, ClientAuthenticationCredentialsValidator>();
        services.AddTransient<IValidator<IdentityEnrollmentCredentials>, IdentityEnrollmentCredentialsValidator>();

        services.AddTransient<IValidator<GroupCreationScheme>, GroupCreationValidator>();
        services.AddTransient<IValidator<GroupUpdateScheme>, GroupUpdateValidator>();
        services.AddTransient<IValidator<AssignGroupPermissionScheme>, AssignGroupPermissionValidator>();

        services.AddTransient<IValidator<PermissionCreationScheme>, PermissionCreationValidator>();
        services.AddTransient<IValidator<PermissionUpdateScheme>, PermissionUpdateValidator>();

        services.AddTransient<IValidator<TenantCreationScheme>, TenantCreationValidator>();
        services.AddTransient<IValidator<TenantUpdateScheme>, TenantUpdateValidator>();

        services.AddTransient<IValidator<AssignUserPermissionScheme>, AssignUserPermissionValidator>();
        services.AddTransient<IValidator<AssignTenantPermissionScheme>, AssignTenantPermissionValidator>();
        services.AddTransient<IValidator<ScopeCreationScheme>, ScopeCreationValidator>();
    }
}
