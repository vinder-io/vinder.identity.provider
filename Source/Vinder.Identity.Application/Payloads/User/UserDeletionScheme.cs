namespace Vinder.Identity.Application.Payloads.User;

public sealed record UserDeletionScheme : IMessage<Result>
{
    public string UserId { get; init; } = default!;
}