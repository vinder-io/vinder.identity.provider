namespace Vinder.IdentityProvider.WebApi.Controllers;

[ApiController]
[TenantRequired]
[Route("api/v1/permissions")]
public sealed class PermissionsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = Permissions.CreatePermission)]
    public async Task<IActionResult> CreatePermissionAsync(PermissionForCreation request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status201Created, result.Data),

            { IsFailure: true } when result.Error == PermissionErrors.PermissionAlreadyExists =>
                StatusCode(StatusCodes.Status409Conflict, result.Error),
        };
    }
}