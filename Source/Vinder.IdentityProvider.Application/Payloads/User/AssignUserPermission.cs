namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record AssignUserPermission : IRequest<Result>
{
    [JsonIgnore]
    public Guid UserId { get; init; }
    public string PermissionName { get; init; } = default!;
}
