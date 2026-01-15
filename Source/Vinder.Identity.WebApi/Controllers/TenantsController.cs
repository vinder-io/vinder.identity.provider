namespace Vinder.Identity.WebApi.Controllers;

[ApiController]
[TenantRequired]
[Route("api/v1/tenants")]
public sealed class TenantsController(IDispatcher dispatcher) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = Permissions.ViewTenants)]
    public async Task<IActionResult> GetTenantsAsync([FromQuery] TenantFetchParameters request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        // we know the switch here is not strictly necessary since we only handle the success case,
        // but we keep it for consistency with the rest of the codebase and to follow established patterns.
        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status200OK, result.Data),
        };
    }

    [HttpPost]
    [Authorize(Roles = Permissions.CreateTenant)]
    public async Task<IActionResult> CreateTenantAsync(TenantCreationScheme request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status201Created, result.Data),

            { IsFailure: true } when result.Error == TenantErrors.TenantAlreadyExists =>
                StatusCode(StatusCodes.Status409Conflict, result.Error),
        };
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Permissions.EditTenant)]
    public async Task<IActionResult> UpdateTenantAsync(
        string id, TenantUpdateScheme request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { TenantId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == TenantErrors.TenantDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Permissions.DeleteTenant)]
    public async Task<IActionResult> DeleteTenantAsync(string id, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(new TenantDeletionScheme { TenantId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status204NoContent),

            { IsFailure: true } when result.Error == TenantErrors.TenantDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }

    [HttpGet("{id}/permissions")]
    [Authorize(Roles = Permissions.ViewPermissions)]
    public async Task<IActionResult> GetTenantPermissionsAsync(
        [FromRoute] string id, [FromQuery] ListTenantAssignedPermissionsParameters request, CancellationToken cancellation
    )
    {
        var result = await dispatcher.DispatchAsync(request with { TenantId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } => StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == TenantErrors.TenantDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),
        };
    }

    [HttpPost("{id}/permissions")]
    [Authorize(Roles = Permissions.AssignPermissions)]
    public async Task<IActionResult> AssignPermissionAsync(
        [FromRoute] string id, [FromBody] AssignTenantPermissionScheme request, CancellationToken cancellation)
    {
        var result = await dispatcher.DispatchAsync(request with { TenantId = id }, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status200OK, result.Data),

            { IsFailure: true } when result.Error == TenantErrors.TenantDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            { IsFailure: true } when result.Error == PermissionErrors.PermissionDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            { IsFailure: true } when result.Error == TenantErrors.TenantAlreadyHasPermission =>
                StatusCode(StatusCodes.Status409Conflict, result.Error),
        };
    }

    [HttpDelete("{id}/permissions/{permissionId}")]
    [Authorize(Roles = Permissions.RevokePermissions)]
    public async Task<IActionResult> RevokePermissionAsync([FromRoute] string id, [FromRoute] string permissionId, CancellationToken cancellation)
    {
        var request = new RevokeTenantPermissionScheme { TenantId = id, PermissionId = permissionId };
        var result = await dispatcher.DispatchAsync(request, cancellation);

        return result switch
        {
            { IsSuccess: true } =>
                StatusCode(StatusCodes.Status204NoContent),

            { IsFailure: true } when result.Error == TenantErrors.TenantDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            { IsFailure: true } when result.Error == PermissionErrors.PermissionDoesNotExist =>
                StatusCode(StatusCodes.Status404NotFound, result.Error),

            { IsFailure: true } when result.Error == TenantErrors.PermissionNotAssigned =>
                StatusCode(StatusCodes.Status409Conflict, result.Error)
        };
    }
}