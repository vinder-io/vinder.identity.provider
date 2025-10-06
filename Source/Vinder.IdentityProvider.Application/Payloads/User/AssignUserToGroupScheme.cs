namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record AssignUserToGroupScheme : IRequest<Result>
{
    [JsonIgnore]
    public string UserId { get; init; } = default!;
    public string GroupId { get; init; } = default!;
}