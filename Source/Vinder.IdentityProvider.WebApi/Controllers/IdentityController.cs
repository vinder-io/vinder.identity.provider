namespace Vinder.IdentityProvider.WebApi.Controllers;

[ApiController]
[Route("api/v1/identity")]
public sealed class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPost("authenticate")]
    [TenantRequired]
    public async Task<IActionResult> AuthenticateAsync(AuthenticationCredentials request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == AuthenticationErrors.InvalidCredentials =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.UserNotFound =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }
}