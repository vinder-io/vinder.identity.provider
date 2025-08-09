namespace Vinder.IdentityProvider.Application.Handlers.Identity;

public sealed class AuthenticationHandler(IAuthenticationService authenticationService) :
    IRequestHandler<AuthenticationCredentials, Result<AuthenticationResult>>
{
    public async Task<Result<AuthenticationResult>> Handle(AuthenticationCredentials credentials, CancellationToken cancellation)
    {
        return await authenticationService.AuthenticateAsync(credentials, cancellation: cancellation);
    }
}