namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed record GroupForDeletion : IRequest<Result>
{
    public Guid GroupId { get; init; }
}
