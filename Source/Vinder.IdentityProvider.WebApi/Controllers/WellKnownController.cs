namespace Vinder.IdentityProvider.WebApi.Controllers;

[ApiController]
[Route(".well-known")]
public sealed class WellKnownController(IMediator mediator) : ControllerBase
{
    [HttpGet("openid-configuration")]
    public async Task<IActionResult> GetConfigurationAsync(
        [FromQuery] FetchOpenIDConfigurationParameters request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        // we know the switch here is not strictly necessary since we only handle the success case,
        // but we keep it for consistency with the rest of the codebase and to follow established patterns.
        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status200OK, result.Data),
        };
    }

    [HttpGet("jwks.json")]
    public async Task<IActionResult> GetJsonWebKeysAsync(
        [FromQuery] FetchJsonWebKeysParameters request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        // we know the switch here is not strictly necessary since we only handle the success case,
        // but we keep it for consistency with the rest of the codebase and to follow established patterns.
        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status200OK, result.Data),
        };
    }
}
