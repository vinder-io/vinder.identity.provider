namespace Vinder.IdentityProvider.WebApi.Controllers;

[ApiController]
[TenantRequired]
[Route("api/v1/users")]
public sealed class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUsersAsync([FromQuery] UsersFetchParameters request, CancellationToken cancellation)
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