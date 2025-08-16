namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed record GroupForUpdate : IRequest<Result<GroupDetails>>
{
    [JsonIgnore]
    public Guid GroupId { get; init; }
    public string Name { get; init; } = default!;
}