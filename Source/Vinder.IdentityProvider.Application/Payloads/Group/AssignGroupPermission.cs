namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed record AssignGroupPermission : IRequest<Result<GroupDetails>>
{
    [JsonIgnore]
    public Guid GroupId { get; init; }

    public string PermissionName { get; init; } = default!;
}
