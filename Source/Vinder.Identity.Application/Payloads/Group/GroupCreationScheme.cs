namespace Vinder.Identity.Application.Payloads.Group;

public sealed record GroupCreationScheme : IRequest<Result<GroupDetailsScheme>>
{
    public string Name { get; init; } = default!;
}