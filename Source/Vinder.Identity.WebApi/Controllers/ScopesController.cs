namespace Vinder.Identity.WebApi.Controllers;

[ApiController]
[TenantRequired]
[Route("api/v1/scopes")]
public sealed class ScopesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = Permissions.CreateScope)]
    public async Task<IActionResult> CreateScopeAsync(ScopeCreationScheme request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status201Created, result.Data),

            { IsFailure: true } when result.Error == ScopeErrors.ScopeAlreadyExists =>
                StatusCode(StatusCodes.Status409Conflict, result.Error),
        };
    }
}