namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed record GroupForUpdate : IRequest<Result<GroupDetails>>
{
    [JsonIgnore]
    public string GroupId { get; init; } = default!;
    public string Name { get; init; } = default!;
}