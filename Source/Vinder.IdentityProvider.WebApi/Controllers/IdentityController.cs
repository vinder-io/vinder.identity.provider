namespace Vinder.IdentityProvider.WebApi.Controllers;

[ApiController]
[Route("api/v1/identity")]
public sealed class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateAsync(AuthenticationCredentials request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        return result switch
        {
            { IsSuccess: true } => Ok(result.Data),

            { IsFailure: true } when result.Error == AuthenticationErrors.InvalidCredentials =>
                Unauthorized(result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.UserNotFound =>
                NotFound(result.Error),
        };
    }
}