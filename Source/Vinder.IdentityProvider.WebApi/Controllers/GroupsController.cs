namespace Vinder.IdentityProvider.WebApi.Controllers;

[ApiController]
[TenantRequired]
[Route("api/v1/groups")]
public sealed class GroupsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = Permissions.ViewGroups)]
    public async Task<IActionResult> GetGroupsAsync([FromQuery] GroupsFetchParameters request, CancellationToken cancellation)
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
    [Authorize(Roles = Permissions.CreateGroup)]
    public async Task<IActionResult> CreateGroupAsync(GroupForCreation request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status201Created, result.Data),

            { IsFailure: true } when result.Error == GroupErrors.GroupAlreadyExists =>
                StatusCode(StatusCodes.Status409Conflict, result.Error),
        };
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Permissions.EditGroup)]
    public async Task<IActionResult> UpdateGroupAsync(string id, GroupForUpdate request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request with { GroupId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == GroupErrors.GroupDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Permissions.DeleteGroup)]
    public async Task<IActionResult> DeleteGroupAsync(string id, CancellationToken cancellation)
    {
        var result = await mediator.Send(new GroupForDeletion { GroupId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status204NoContent),

            { IsFailure: true } when result.Error == GroupErrors.GroupDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }

    [HttpGet("{id}/permissions")]
    [Authorize(Roles = Permissions.ViewPermissions)]
    public async Task<IActionResult> GetGroupsPermissionsAsync(
        [FromRoute] string id,
        [FromQuery] ListGroupAssignedPermissions request, CancellationToken cancellation
    )
    {
        var result = await mediator.Send(request with { GroupId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == GroupErrors.GroupDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }

    [HttpPost("{id}/permissions")]
    [Authorize(Roles = Permissions.AssignPermissions)]
    public async Task<IActionResult> AssignPermissionAsync(string id, AssignGroupPermission request, CancellationToken cancellation)
    {
        var result = await mediator.Send(request with { GroupId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == GroupErrors.GroupDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            { IsFailure: true } when result.Error == PermissionErrors.PermissionDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            { IsFailure: true } when result.Error == GroupErrors.GroupAlreadyHasPermission =>
                StatusCode(StatusCodes.Status409Conflict, result.Error),
        };
    }

    [HttpDelete("{id}/permissions/{permissionId}")]
    [Authorize(Roles = Permissions.RevokePermissions)]
    public async Task<IActionResult> RevokePermissionAsync(string id, string permissionId, CancellationToken cancellation)
    {
        var request = new RevokeGroupPermission { GroupId = id, PermissionId = permissionId };
        var result = await mediator.Send(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status204NoContent),

            { IsFailure: true } when result.Error == GroupErrors.GroupDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            { IsFailure: true } when result.Error == PermissionErrors.PermissionDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            { IsFailure: true } when result.Error == GroupErrors.PermissionNotAssigned =>
                StatusCode(StatusCodes.Status409Conflict, result.Error)
        };
    }
}