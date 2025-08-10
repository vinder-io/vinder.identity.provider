namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed record GroupForCreation : IRequest<Result<GroupDetails>>
{
    public string Name { get; init; } = default!;
}