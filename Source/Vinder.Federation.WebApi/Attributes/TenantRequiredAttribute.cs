namespace Vinder.Federation.WebApi.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class TenantRequiredAttribute : Attribute;