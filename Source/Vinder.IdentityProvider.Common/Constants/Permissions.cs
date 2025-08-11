namespace Vinder.IdentityProvider.Common.Constants;

public static class Permissions
{
    public const string CreateGroup = "vinder.default.permissions.group.create";
    public const string DeleteGroup = "vinder.default.permissions.group.delete";
    public const string EditGroup = "vinder.default.permissions.group.update";
    public const string ViewGroups = "vinder.default.permissions.group.view";

    public const string CreateClient = "vinder.default.permissions.client.create";
    public const string DeleteClient = "vinder.default.permissions.client.delete";
    public const string EditClient = "vinder.default.permissions.client.update";
    public const string ViewClients = "vinder.default.permissions.client.view";

    public const string AssignPermissions = "vinder.default.permissions.permissions.assign";
    public const string RevokePermissions = "vinder.default.permissions.permissions.revoke";
    public const string ViewPermissions = "vinder.default.permissions.permissions.view";

    public const string CreateTenant = "vinder.default.permissions.tenant.create";
    public const string DeleteTenant = "vinder.default.permissions.tenant.delete";
    public const string EditTenant = "vinder.default.permissions.tenant.update";
    public const string ViewTenants = "vinder.default.permissions.tenant.view";
}