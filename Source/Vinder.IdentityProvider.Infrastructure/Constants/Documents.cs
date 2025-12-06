namespace Vinder.IdentityProvider.Infrastructure.Constants;

public static class Documents
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

    public static class Group
    {
        public const string Id = "_id";
        public const string Name = nameof(Domain.Entities.Group.Name);
        public const string TenantId = nameof(Domain.Entities.Group.TenantId);
        public const string IsDeleted = nameof(Domain.Entities.Group.IsDeleted);
    }

    public static class Scope
    {
        public const string Id = "_id";
        public const string TenantId = nameof(Domain.Entities.Scope.TenantId);
        public const string Name = nameof(Domain.Entities.Scope.Name);
        public const string IsDeleted = nameof(Domain.Entities.Group.IsDeleted);
    }

    public static class Tenant
    {
        public const string Name = nameof(Domain.Entities.Tenant.Name);
        public const string ClientId = nameof(Domain.Entities.Tenant.ClientId);
        public const string IsDeleted = nameof(Domain.Entities.Tenant.IsDeleted);
        public const string Id = "_id";
    }

    public static class SecurityToken
    {
        public const string Value = nameof(Domain.Entities.SecurityToken.Value);
        public const string Type = nameof(Domain.Entities.SecurityToken.Type);
        public const string UserId = nameof(Domain.Entities.SecurityToken.UserId);
        public const string TenantId = nameof(Domain.Entities.SecurityToken.TenantId);
        public const string IsDeleted = nameof(Domain.Entities.SecurityToken.IsDeleted);
        public const string Id = "_id";
    }
}
