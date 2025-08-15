namespace Vinder.IdentityProvider.Application.Payloads.Permission;

public sealed record PermissionForUpdate : IRequest<Result<PermissionDetails>>
{
    [JsonIgnore]
    public Guid PermissionId { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; } = default!;
}