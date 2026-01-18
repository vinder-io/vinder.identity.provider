namespace Vinder.Federation.Application.Payloads.User;

public sealed record AssignUserToGroupScheme : IMessage<Result>
{
    [JsonIgnore]
    public string UserId { get; init; } = default!;
    public string GroupId { get; init; } = default!;
}