namespace Vinder.IdentityProvider.WebApi.Controllers;

[ApiController]
[TenantRequired]
[Route("api/v1/users")]
public sealed class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = Permissions.ViewUsers)]
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

    [HttpGet("{id:guid}/permissions")]
    [Authorize(Roles = Permissions.ViewPermissions)]
    public async Task<IActionResult> GetUserPermissionsAsync(
        [FromRoute] Guid id,
        [FromQuery] ListUserAssignedPermissions request, CancellationToken cancellation
    )
    {
        var result = await mediator.Send(request with { UserId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == UserErrors.UserDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }

    [HttpGet("{id:guid}/groups")]
    [Authorize(Roles = Permissions.ViewGroups)]
    public async Task<IActionResult> GetUserGroupsAsync(
        [FromRoute] Guid id,
        [FromQuery] ListUserAssignedGroups request, CancellationToken cancellation
    )
    {
        var result = await mediator.Send(request with { UserId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == UserErrors.UserDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }

    [HttpPost("{id:guid}/groups")]
    [Authorize(Roles = Permissions.EditUser)]
    public async Task<IActionResult> AssignUserToGroupAsync(Guid id, AssignUserToGroup request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request with { UserId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status204NoContent),

            { IsFailure: true } when result.Error == UserErrors.UserDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            { IsFailure: true } when result.Error == UserErrors.UserAlreadyInGroup =>
                StatusCode(StatusCodes.Status409Conflict, result.Error),

            { IsFailure: true } when result.Error == GroupErrors.GroupDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }

    [HttpPost("{id:guid}/permissions")]
    [Authorize(Roles = Permissions.AssignPermissions)]
    public async Task<IActionResult> AssignUserPermissionAsync(Guid id, AssignUserPermission request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request with { UserId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status204NoContent),

            { IsFailure: true } when result.Error == UserErrors.UserDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            { IsFailure: true } when result.Error == PermissionErrors.PermissionDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            { IsFailure: true } when result.Error == UserErrors.UserAlreadyHasPermission =>
                StatusCode(StatusCodes.Status409Conflict, result.Error),
        };
    }

    [HttpDelete("{id:guid}/permissions/{permissionId:guid}")]
    [Authorize(Roles = Permissions.RevokePermissions)]
    public async Task<IActionResult> RevokePermissionAsync(Guid id, Guid permissionId, CancellationToken cancellation)
    {
        var request = new RevokeUserPermission { UserId = id, PermissionId = permissionId };
        var result = await mediator.Send(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status204NoContent),

            { IsFailure: true } when result.Error == UserErrors.UserDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            { IsFailure: true } when result.Error == PermissionErrors.PermissionDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            { IsFailure: true } when result.Error == UserErrors.PermissionNotAssigned =>
                StatusCode(StatusCodes.Status409Conflict, result.Error)
        };
    }
}