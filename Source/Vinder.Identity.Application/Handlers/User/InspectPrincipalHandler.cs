namespace Vinder.Identity.Application.Handlers.User;

public sealed class InspectPrincipalHandler(IPrincipalProvider principalProvider) :
    IRequestHandler<InspectPrincipalParameters, Result<PrincipalDetailsScheme>>
{
    public Task<Result<PrincipalDetailsScheme>> Handle(
        InspectPrincipalParameters request, CancellationToken cancellationToken)
    {
        var user = principalProvider.GetCurrentPrincipal();

        return user is not null
            ? Task.FromResult(Result<PrincipalDetailsScheme>.Success(UserMapper.AsPrincipal(user)))
            : Task.FromResult(Result<PrincipalDetailsScheme>.Failure(UserErrors.UserDoesNotExist));
    }
}
