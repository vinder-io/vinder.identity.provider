namespace Vinder.IdentityProvider.WebApi.Controllers;

[ApiController]
[Route("api/v1/identity")]
public sealed class IdentityController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [TenantRequired]
    public async Task<IActionResult> EnrollIdentityAsync(IdentityEnrollmentCredentials request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status201Created),

            { IsFailure: true } when result.Error == IdentityErrors.UserAlreadyExists =>
                StatusCode(StatusCodes.Status409Conflict, result.Error),
        };
    }

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

    [HttpPost("refresh-token")]
    [TenantRequired]
    public async Task<IActionResult> RefreshTokenAsync(SessionTokenRenewal request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == AuthenticationErrors.InvalidRefreshToken =>
                StatusCode(StatusCodes.Status400BadRequest, result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.TokenExpired =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.InvalidSignature =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.InvalidTokenFormat =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.UserNotFound =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }

    [HttpPost("invalidate-session")]
    [TenantRequired]
    public async Task<IActionResult> InvalidateSessionAsync(SessionInvalidation request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status204NoContent),

            { IsFailure: true } when result.Error == AuthenticationErrors.InvalidRefreshToken =>
                StatusCode(StatusCodes.Status400BadRequest, result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.TokenExpired =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.InvalidSignature =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.LogoutFailed =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),

            { IsFailure: true } when result.Error == AuthenticationErrors.InvalidTokenFormat =>
                StatusCode(StatusCodes.Status401Unauthorized, result.Error),
        };
    }
}