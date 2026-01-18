namespace Vinder.Federation.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage]
public static class MediatorExtension
{
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddDispatcher(typeof(AuthenticationHandler));
    }
}