namespace Vinder.IdentityProvider.Infrastructure.Constants;

public static class DocumentFields
{
    public static class User
    {
        public const string IsDeleted = nameof(Domain.Entities.User.IsDeleted);
        public const string Username = nameof(Domain.Entities.User.Username);
        public const string TenantId = nameof(Domain.Entities.User.TenantId);
        public const string Id = "_id";
    }

    public static class Permission
    {
        public const string Name = nameof(Domain.Entities.Permission.Name);
        public const string Description = nameof(Domain.Entities.Permission.Description);
        public const string IsDeleted = nameof(Domain.Entities.Permission.IsDeleted);
        public const string TenantId = nameof(Domain.Entities.Permission.TenantId);
        public const string Id = "_id";
    }

    public static class PermissionGroup
    {
        public const string Id = "_id";
        public const string TenantId = nameof(Domain.Entities.PermissionGroup.TenantId);
        public const string Name = nameof(Domain.Entities.PermissionGroup.Name);
    }

    public static class SecurityToken
    {
        public const string Value = nameof(Domain.Entities.SecurityToken.Value);
        public const string Type = nameof(Domain.Entities.SecurityToken.Type);
        public const string UserId = nameof(Domain.Entities.SecurityToken.UserId);
        public const string TenantId = nameof(Domain.Entities.SecurityToken.TenantId);
        public const string Id = "_id";
    }
}
