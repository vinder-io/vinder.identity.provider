namespace Vinder.IdentityProvider.Infrastructure.IoC.Extensions;

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
        services.AddTransient<IValidator<ClientAuthenticationCredentials>, ClientAuthenticationCredentialsValidator>();
        services.AddTransient<IValidator<IdentityEnrollmentCredentials>, IdentityEnrollmentCredentialsValidator>();

        services.AddTransient<IValidator<GroupForCreation>, GroupCreationValidator>();
        services.AddTransient<IValidator<GroupForUpdate>, GroupUpdateValidator>();
        services.AddTransient<IValidator<AssignGroupPermission>, AssignGroupPermissionValidator>();

        services.AddTransient<IValidator<PermissionForCreation>, PermissionCreationValidator>();
        services.AddTransient<IValidator<PermissionForUpdate>, PermissionUpdateValidator>();
        services.AddTransient<IValidator<TenantForCreation>, TenantCreationValidator>();
    }
}