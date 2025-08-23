namespace Vinder.IdentityProvider.WebApi.Controllers;

[ApiController]
[Route("api/v1/tenants")]
public sealed class TenantsController(IMediator mediator) : ControllerBase
{
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