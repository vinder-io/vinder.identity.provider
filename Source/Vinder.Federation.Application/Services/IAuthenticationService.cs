namespace Vinder.Federation.Application.Services;

public interface IAuthenticationService
{
    public Task<Result<AuthenticationResult>> AuthenticateAsync(
        AuthenticationCredentials credentials,
        CancellationToken cancellation = default
    );
}