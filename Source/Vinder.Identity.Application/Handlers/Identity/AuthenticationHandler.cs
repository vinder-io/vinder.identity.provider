namespace Vinder.Identity.Application.Handlers.Identity;

public sealed class AuthenticationHandler(IAuthenticationService authenticationService) :
    IRequestHandler<AuthenticationCredentials, Result<AuthenticationResult>>
{
    public async Task<Result<AuthenticationResult>> Handle(AuthenticationCredentials credentials, CancellationToken cancellationToken)
    {
        return await authenticationService.AuthenticateAsync(credentials, cancellation: cancellationToken);
    }
}