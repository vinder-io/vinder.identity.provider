namespace Vinder.Federation.Application.Payloads.User;

public sealed record UserDeletionScheme : IMessage<Result>
{
    public string UserId { get; init; } = default!;
}