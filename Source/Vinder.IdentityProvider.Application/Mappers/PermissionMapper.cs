namespace Vinder.IdentityProvider.Application.Mappers;

public static class PermissionMapper
{
    public static PermissionDetails AsResponse(Permission permission) => new()
    {
        Id = permission.Id.ToString(),
        Name = permission.Name
    };
}