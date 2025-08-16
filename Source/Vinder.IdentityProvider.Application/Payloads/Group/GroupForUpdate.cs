namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed class GroupForUpdate : IRequest<Result<GroupDetails>>
{
    [JsonIgnore]
    public Guid GroupId { get; init; }

    public string Name { get; init; } = default!;
}