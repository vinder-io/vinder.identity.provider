namespace Vinder.Identity.WebApi.Controllers;

[ApiController]
[Route("api/v1/openid/connect")]
public sealed class ConnectController(IMediator mediator) : ControllerBase
{
    [HttpPost("token")]
    public async Task<IActionResult> AuthenticateClientAsync(
        [FromSnakeCaseForm] ClientAuthenticationCredentials request,
        CancellationToken cancellation
    )
    {
        var result = await mediator.Send(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == AuthenticationErrors.ClientNotFound =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.InvalidClientCredentials =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),
        };
    }
}