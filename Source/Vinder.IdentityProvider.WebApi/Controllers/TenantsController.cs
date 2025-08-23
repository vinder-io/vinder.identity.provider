namespace Vinder.IdentityProvider.WebApi.Controllers;

[ApiController]
[Route("api/v1/tenants")]
public sealed class TenantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = Permissions.ViewTenants)]
    public async Task<IActionResult> GetTenantsAsync([FromQuery] TenantFetchParameters request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        // we know the switch here is not strictly necessary since we only handle the success case,
        // but we keep it for consistency with the rest of the codebase and to follow established patterns.
        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status200OK, result.Data),
        };
    }

    [HttpPost]
    [Authorize(Roles = Permissions.CreateTenant)]
    public async Task<IActionResult> CreateTenantAsync(TenantForCreation request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status201Created, result.Data),

            { IsFailure: true } when result.Error == TenantErrors.TenantAlreadyExists =>
                StatusCode(StatusCodes.Status409Conflict, result.Error),
        };
    }
}