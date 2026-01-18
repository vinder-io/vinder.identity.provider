namespace Vinder.Federation.Application.Payloads.Group;

public sealed record GroupDeletionScheme : IMessage<Result>
{
    public string GroupId { get; init; } = default!;
}
