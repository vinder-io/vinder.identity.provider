namespace Vinder.Identity.Application.Handlers.User;

public sealed class InspectPrincipalHandler(IPrincipalProvider principalProvider) :
    IMessageHandler<InspectPrincipalParameters, Result<PrincipalDetailsScheme>>
{
    public Task<Result<PrincipalDetailsScheme>> HandleAsync(
        InspectPrincipalParameters parameters, CancellationToken cancellation)
    {
        var user = principalProvider.GetCurrentPrincipal();

        return user is not null
            ? Task.FromResult(Result<PrincipalDetailsScheme>.Success(UserMapper.AsPrincipal(user)))
            : Task.FromResult(Result<PrincipalDetailsScheme>.Failure(UserErrors.UserDoesNotExist));
    }
}
