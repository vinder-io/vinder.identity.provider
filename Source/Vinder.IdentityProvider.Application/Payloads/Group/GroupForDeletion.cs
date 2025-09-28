namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed record GroupForDeletion : IRequest<Result>
{
    public string GroupId { get; init; } = default!;
}
