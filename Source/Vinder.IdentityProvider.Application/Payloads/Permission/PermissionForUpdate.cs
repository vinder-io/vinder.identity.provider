namespace Vinder.IdentityProvider.Application.Payloads.Permission;

public sealed record PermissionForUpdate : IRequest<Result<PermissionDetails>>
{
    [JsonIgnore]
    public string PermissionId { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string? Description { get; init; } = default!;
}