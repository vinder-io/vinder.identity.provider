namespace Vinder.IdentityProvider.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage]
public static class MediatorExtension
{
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining<AuthenticationHandler>();
        });
    }
}