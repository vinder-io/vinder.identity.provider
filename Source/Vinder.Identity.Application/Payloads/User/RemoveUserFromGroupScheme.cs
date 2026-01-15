namespace Vinder.Identity.Application.Payloads.User;

public sealed record RemoveUserFromGroupScheme : IMessage<Result>
{
    public string UserId { get; init; } = default!;
    public string GroupId { get; init; } = default!;
}
