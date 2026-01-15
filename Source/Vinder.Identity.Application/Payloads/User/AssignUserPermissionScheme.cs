namespace Vinder.Identity.Application.Payloads.User;

public sealed record AssignUserPermissionScheme : IRequest<Result>
{
    [JsonIgnore]
    public string UserId { get; init; } = default!;
    public string PermissionName { get; init; } = default!;
}
