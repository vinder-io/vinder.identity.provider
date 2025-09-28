namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record AssignUserPermission : IRequest<Result>
{
    [JsonIgnore]
    public string UserId { get; init; } = default!;
    public string PermissionName { get; init; } = default!;
}
