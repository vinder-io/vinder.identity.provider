namespace Vinder.Identity.Application.Payloads.Group;

public sealed record GroupDeletionScheme : IRequest<Result>
{
    public string GroupId { get; init; } = default!;
}
