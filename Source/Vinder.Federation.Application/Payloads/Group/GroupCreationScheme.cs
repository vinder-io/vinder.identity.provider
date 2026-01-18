namespace Vinder.Federation.Application.Payloads.Group;

public sealed record GroupCreationScheme : IMessage<Result<GroupDetailsScheme>>
{
    public string Name { get; init; } = default!;
}