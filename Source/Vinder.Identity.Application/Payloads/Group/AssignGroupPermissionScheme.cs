namespace Vinder.Identity.Application.Payloads.Group;

public sealed record AssignGroupPermissionScheme : IMessage<Result<GroupDetailsScheme>>
{
    [JsonIgnore]
    public string GroupId { get; init; } = default!;
    public string PermissionName { get; init; } = default!;
}
