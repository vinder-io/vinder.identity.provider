namespace Vinder.IdentityProvider.WebApi.Controllers;

[ApiController]
[TenantRequired]
[Route("api/v1/groups")]
public sealed class GroupsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = Permissions.CreateGroup)]
    public async Task<IActionResult> CreateGroupAsync(GroupForCreation request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status201Created),

            { IsFailure: true } when result.Error == GroupErrors.GroupAlreadyExists =>
                StatusCode(StatusCodes.Status409Conflict, result.Error),
        };
    }
}