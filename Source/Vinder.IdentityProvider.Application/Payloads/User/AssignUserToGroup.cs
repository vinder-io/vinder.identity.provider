namespace Vinder.IdentityProvider.Application.Payloads.User;

public sealed record AssignUserToGroup : IRequest<Result>
{
    [JsonIgnore]
    public Guid UserId { get; init; }
    public Guid GroupId { get; init; }
}