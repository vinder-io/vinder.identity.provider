namespace Vinder.Identity.WebApi.Controllers;

[ApiController]
[Route("api/v1/openid/connect")]
public sealed class ConnectController(IDispatcher dispatcher) : ControllerBase
{
    [HttpPost("token")]
    public async Task<IActionResult> AuthenticateClientAsync(
        [FromSnakeCaseForm] ClientAuthenticationCredentials request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == AuthenticationErrors.ClientNotFound =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.InvalidAuthorizationCode =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.AuthorizationCodeExpired =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.InvalidCodeVerifier =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.InvalidClientCredentials =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),
        };
    }
}