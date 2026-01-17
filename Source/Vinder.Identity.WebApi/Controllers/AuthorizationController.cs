namespace Vinder.Identity.WebApi.Controllers;

[ApiController]
[Route("oauth2")]
public sealed class AuthorizationController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet("authorize")]
    public async Task<IActionResult> AuthorizeAsync(
        [FromQuery] AuthorizationParameters request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        return result switch
        {
            { IsSuccess: true } when result.Data is not null =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } =>
                BadRequest(result.Error),
        };
    }
}
