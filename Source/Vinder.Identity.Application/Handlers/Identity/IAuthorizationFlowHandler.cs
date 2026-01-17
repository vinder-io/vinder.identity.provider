namespace Vinder.Identity.Application.Handlers.Identity;

public interface IAuthorizationFlowHandler
{
    Task<Result<ClientAuthenticationResult>> HandleAsync(ClientAuthenticationCredentials parameters, CancellationToken cancellation = default);
}
