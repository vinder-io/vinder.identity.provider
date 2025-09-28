namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed record AssignGroupPermission : IRequest<Result<GroupDetails>>
{
    [JsonIgnore]
    public string GroupId { get; init; } = default!;
    public string PermissionName { get; init; } = default!;
}
