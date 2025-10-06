namespace Vinder.IdentityProvider.Application.Payloads.Group;

public sealed record GroupUpdateScheme : IRequest<Result<GroupDetailsScheme>>
{
    [JsonIgnore]
    public string GroupId { get; init; } = default!;
    public string Name { get; init; } = default!;
}