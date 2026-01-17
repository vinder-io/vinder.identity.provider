namespace Vinder.Identity.Application.Handlers.Identity;

public sealed class AuthenticationHandler(IAuthenticationService authenticationService) :
    IMessageHandler<AuthenticationCredentials, Result<AuthenticationResult>>
{
    public async Task<Result<AuthenticationResult>> HandleAsync(AuthenticationCredentials credentials, CancellationToken cancellation = default)
    {
        return await authenticationService.AuthenticateAsync(credentials, cancellation: cancellation);
    }
}